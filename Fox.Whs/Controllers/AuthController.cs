using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fox.Whs.Dtos;
using Fox.Whs.Services;
using System.Security.Claims;

namespace Fox.Whs.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(
        AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login người dùng - Xác thực qua SAP và trả về JWT token
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLogin loginDto)
    {

        // Xác thực qua AuthService (sẽ gọi SAP bên trong)
        var result = await _authService.AuthenticateAsync(
            loginDto.Username,
            loginDto.Password);


        return Ok(result);
    }

    /// <summary>
    /// Logout người dùng (chỉ để thông báo, token sẽ tự hết hạn)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest? request)
    {
        // Thu hồi refresh token nếu có
        if (request?.RefreshToken != null)
        {
            await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
        }

        return Ok(new
        {
            success = true,
            message = "Đăng xuất thành công. Vui lòng xóa token ở phía client."
        });
    }

    /// <summary>
    /// Làm mới access token bằng refresh token
    /// </summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken);
        return Ok(result);
    }

    /// <summary>
    /// Kiểm tra token có hợp lệ không
    /// </summary>
    [HttpGet("validate-token")]
    [Authorize]
    public IActionResult ValidateToken()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;

        return Ok(new
        {
            success = true,
            message = "Token hợp lệ",
            data = new
            {
                userId = userId,
                username = username,
                isAuthenticated = User.Identity?.IsAuthenticated ?? false
            }
        });
    }
}
