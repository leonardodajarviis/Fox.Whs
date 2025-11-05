using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Fox.Whs.Services;
using Fox.Whs.Exceptions;
using Fox.Whs.Options;
using Microsoft.Extensions.Options;

namespace Fox.Whs.Filters;

/// <summary>
/// Filter để kiểm tra Access Token JTI có hợp lệ không
/// Đảm bảo chỉ có access token cuối cùng được tạo mới có thể sử dụng
/// </summary>
public class ValidateAccessTokenJtiFilter : IAsyncActionFilter
{
    private readonly AuthService _authService;
    private readonly JwtOptions _jwtOptions;

    public ValidateAccessTokenJtiFilter(AuthService authService, IOptions<JwtOptions> jwtOptions)
    {
        
        _authService = authService;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Bỏ qua cho các endpoint không cần authentication
        var endpoint = context.HttpContext.GetEndpoint();
        var hasAllowAnonymous = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null;
        
        if (hasAllowAnonymous)
        {
            await next();
            return;
        }

        // Bỏ qua nếu không có Authorization header
        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            await next();
            return;
        }

        var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
        
        // Kiểm tra Bearer token
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            await next();
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        // Kiểm tra JTI có hợp lệ không
        var isValid = await _authService.ValidateAccessTokenJtiAsync(token);

        if (!isValid)
        {
            throw new UnauthorizedException("Access token không còn hợp lệ. Vui lòng làm mới token hoặc đăng nhập lại.");
        }

        await next();
    }
}
