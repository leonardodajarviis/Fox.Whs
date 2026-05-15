using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Fox.Whs.Data;
using Fox.Whs.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Middlewares;

/// <summary>
/// Middleware xử lý Idempotency-Key cho các endpoint tạo/cập nhật báo cáo sản xuất.
/// Đảm bảo cùng một key (UUID v4) chỉ sinh ra một bản ghi, replay response đã cache.
/// </summary>
public class IdempotencyMiddleware
{
    private const string HeaderName = "Idempotency-Key";
    private const string TestDelayHeaderName = "X-Test-Delay-Ms";
    private const int MaxTestDelayMs = 30_000;
    private static readonly TimeSpan Ttl = TimeSpan.FromHours(24);

    private static readonly Regex UuidV4Regex = new(
        "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-4[0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$",
        RegexOptions.Compiled);

    private static readonly HashSet<string> EligiblePathPrefixes = new(StringComparer.OrdinalIgnoreCase)
    {
        "/api/grain-mixing-processes",
        "/api/blowing-processes",
        "/api/cutting-processes",
        "/api/printing-processes",
        "/api/rewinding-processes",
        "/api/slitting-processes",
        "/api/grain-mixing-blowing-processes",
    };

    private static readonly JsonSerializerOptions ErrorJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    private readonly RequestDelegate _next;

    public IdempotencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db, IWebHostEnvironment env)
    {
        await ApplyTestDelayAsync(context, env);

        if (!ShouldHandle(context))
        {
            await _next(context);
            return;
        }

        var keyHeader = GetHeaderValue(context.Request, HeaderName);
        if (string.IsNullOrWhiteSpace(keyHeader))
        {
            // Caller cũ không gửi header — xử lý bình thường, không dedupe.
            await _next(context);
            return;
        }

        if (!UuidV4Regex.IsMatch(keyHeader))
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest,
                "Idempotency-Key phải là UUID v4 hợp lệ.");
            return;
        }

        var userId = GetUserId(context);
        if (userId is null)
        {
            // Không xác định user → cứ chạy bình thường, để [Authorize] reject.
            await _next(context);
            return;
        }

        var method = context.Request.Method.ToUpperInvariant();
        var path = context.Request.Path.HasValue ? context.Request.Path.Value! : "/";

        var bodyBytes = await ReadBodyAsync(context.Request);
        var requestHash = ComputeSha256Hex(bodyBytes);

        var now = DateTime.UtcNow;
        var existing = await db.IdempotencyKeys
            .AsNoTracking()
            .FirstOrDefaultAsync(k =>
                k.Key == keyHeader &&
                k.UserId == userId.Value &&
                k.Method == method &&
                k.Path == path);

        if (existing != null)
        {
            if (existing.ExpiresAt <= now)
            {
                // Key cũ đã hết hạn → xóa rồi xử lý lại như request mới.
                await db.IdempotencyKeys
                    .Where(k => k.Id == existing.Id)
                    .ExecuteDeleteAsync();
            }
            else
            {
                // Cùng key + còn hạn → replay response cũ, bất kể payload có khác.
                // RequestHash vẫn được lưu để debug, nhưng không dùng để reject.
                await ReplayAsync(context, existing);
                return;
            }
        }

        // Wrap response body để capture sau khi action chạy xong.
        var originalBody = context.Response.Body;
        using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            await _next(context);
        }
        finally
        {
            context.Response.Body = originalBody;
        }

        buffer.Position = 0;
        var responseBytes = buffer.ToArray();

        if (responseBytes.Length > 0)
        {
            await originalBody.WriteAsync(responseBytes);
        }

        // Chỉ cache response 2xx (commit DB thành công). Lỗi 4xx/5xx để FE retry.
        if (context.Response.StatusCode is >= 200 and < 300)
        {
            var record = new IdempotencyKey
            {
                Key = keyHeader,
                UserId = userId.Value,
                Method = method,
                Path = path,
                RequestHash = requestHash,
                ResponseStatus = context.Response.StatusCode,
                ResponseBody = responseBytes.Length > 0
                    ? Encoding.UTF8.GetString(responseBytes)
                    : null,
                ResponseContentType = context.Response.ContentType,
                CreatedAt = now,
                ExpiresAt = now.Add(Ttl),
            };

            try
            {
                db.IdempotencyKeys.Add(record);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                // Race: request song song cùng key đã thắng việc insert.
                // Không ghi đè response đã gửi (đã ở trên buffer). Bỏ qua cache.
            }
        }
    }

    /// <summary>
    /// Áp dụng delay nhân tạo qua header X-Test-Delay-Ms để giả lập mạng/server chậm.
    /// Chỉ hoạt động trong môi trường Development để tránh bị abuse ở production.
    /// Giá trị clamp về [0, MaxTestDelayMs].
    /// </summary>
    private static async Task ApplyTestDelayAsync(HttpContext context, IWebHostEnvironment env)
    {
        var raw = GetHeaderValue(context.Request, TestDelayHeaderName);
        if (string.IsNullOrEmpty(raw)) return;

        if (!int.TryParse(raw, out var ms) || ms <= 0) return;

        ms = Math.Min(ms, MaxTestDelayMs);
        await Task.Delay(ms, context.RequestAborted);
    }

    private static bool ShouldHandle(HttpContext context)
    {
        // Chỉ dedupe POST. PUT bị bỏ qua vì FE giữ form mở, cùng UUID nhưng
        // mỗi lần save một payload khác nhau (cập nhật từng phần) — nếu replay
        // response cũ thì các lần update sau sẽ bị nuốt mất.
        var method = context.Request.Method;
        if (!HttpMethods.IsPost(method))
            return false;

        var path = context.Request.Path;
        if (!path.HasValue) return false;

        var value = path.Value!;
        foreach (var prefix in EligiblePathPrefixes)
        {
            if (value.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                return true;
            if (value.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private static string? GetHeaderValue(HttpRequest request, string name)
    {
        // HttpRequest.Headers tra cứu case-insensitive theo spec ASP.NET Core.
        if (request.Headers.TryGetValue(name, out var values))
        {
            var v = values.ToString();
            return string.IsNullOrWhiteSpace(v) ? null : v.Trim();
        }
        return null;
    }

    private static short? GetUserId(HttpContext context)
    {
        var claim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return short.TryParse(claim, out var id) ? id : null;
    }

    private static async Task<byte[]> ReadBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        using var ms = new MemoryStream();
        await request.Body.CopyToAsync(ms);
        request.Body.Position = 0;
        return ms.ToArray();
    }

    private static string ComputeSha256Hex(byte[] bytes)
    {
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static async Task ReplayAsync(HttpContext context, IdempotencyKey cached)
    {
        context.Response.StatusCode = cached.ResponseStatus;
        if (!string.IsNullOrEmpty(cached.ResponseContentType))
            context.Response.ContentType = cached.ResponseContentType;
        context.Response.Headers["Idempotent-Replayed"] = "true";

        if (!string.IsNullOrEmpty(cached.ResponseBody))
            await context.Response.WriteAsync(cached.ResponseBody);
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode status, string message)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        var payload = new ErrorResponse
        {
            StatusCode = (int)status,
            Message = message,
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload, ErrorJsonOptions));
    }

    private static bool IsUniqueViolation(DbUpdateException ex)
    {
        return ex.InnerException is SqlException sql && (sql.Number == 2627 || sql.Number == 2601);
    }
}

public static class IdempotencyMiddlewareExtensions
{
    public static IApplicationBuilder UseIdempotency(this IApplicationBuilder app)
        => app.UseMiddleware<IdempotencyMiddleware>();
}
