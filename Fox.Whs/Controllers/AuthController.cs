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
    public IActionResult Logout()
    {
        // JWT token sẽ tự động hết hạn theo thời gian cấu hình
        // Client cần xóa token ở phía mình

        return Ok(new
        {
            success = true,
            message = "Đăng xuất thành công. Vui lòng xóa token ở phía client."
        });
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
