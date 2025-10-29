using System.Net;
using System.Text.Json;
using Fox.Whs.Exceptions;

namespace Fox.Whs.Middlewares;

/// <summary>
/// Middleware để bắt và xử lý exception toàn cục
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            // Custom Exceptions
            BadRequestException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                Details = ex.InnerException?.Message
            },
            NotFoundException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = ex.Message,
                Details = ex.InnerException?.Message
            },
            UnauthorizedException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = ex.Message,
                Details = ex.InnerException?.Message
            },
            ForbiddenException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                Message = ex.Message,
                Details = ex.InnerException?.Message
            },
            ConflictException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Message = ex.Message,
                Details = ex.InnerException?.Message
            },
            ValidationException ex => new ValidationErrorResponse
            {
                StatusCode = (int)HttpStatusCode.UnprocessableEntity,
                Message = ex.Message,
                Details = "Vui lòng kiểm tra lại dữ liệu đầu vào",
                Errors = ex.Errors
            },
            
            // Built-in Exceptions
            ArgumentException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Tham số không hợp lệ",
                Details = ex.Message
            },
            KeyNotFoundException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Không tìm thấy tài nguyên",
                Details = ex.Message
            },
            UnauthorizedAccessException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Bạn không có quyền truy cập",
                Details = ex.Message
            },
            
            // Default
            _ => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Có lỗi xảy ra trên server",
                Details = exception.Message
            }
        };

        context.Response.StatusCode = response.StatusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

/// <summary>
/// Model cho error response
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Mã trạng thái HTTP
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Thông báo lỗi chính
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Chi tiết lỗi
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Thời gian xảy ra lỗi
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Model cho validation error response (kế thừa từ ErrorResponse)
/// </summary>
public class ValidationErrorResponse : ErrorResponse
{
    /// <summary>
    /// Chi tiết lỗi validation theo từng field
    /// </summary>
    public IDictionary<string, string[]>? Errors { get; set; }
}
