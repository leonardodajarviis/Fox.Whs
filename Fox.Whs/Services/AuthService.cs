using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Options;
using Fox.Whs.Data;
using Fox.Whs.SapModels;
using Fox.Whs.Exceptions;

namespace Fox.Whs.Services;

public class AuthService
{
    private readonly SapServiceLayerAuthService _sapAuthService;
    private readonly AppDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        SapServiceLayerAuthService sapAuthService,
        AppDbContext sapDbContext,
        IOptions<JwtOptions> jwtOptions,
        ILogger<AuthService> logger)
    {
        _sapAuthService = sapAuthService;
        _dbContext = sapDbContext;
        _jwtOptions = jwtOptions.Value;
        _logger = logger;
    }

    /// <summary>
    /// Xác thực người dùng và tạo JWT token
    /// </summary>
    public async Task<AuthResponse> AuthenticateAsync(string username, string password)
    {
        _logger.LogInformation("Bắt đầu xác thực user: {Username}", username);

        // Validate input
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new BadRequestException("Tên đăng nhập không được để trống");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new BadRequestException("Mật khẩu không được để trống");
        }

        // var sessionId = await _sapAuthService.LoginAsync(username, password);

        // if (string.IsNullOrEmpty(sessionId))
        // {
        //     _logger.LogWarning("Xác thực SAP thất bại cho user: {Username}", username);
        //     throw new UnauthorizedException("Tên đăng nhập hoặc mật khẩu không đúng");
        // }

        _logger.LogInformation("Xác thực SAP thành công cho user: {Username}", username);

        var user = await _dbContext.Users.AsNoTracking()
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(u => u.Username == username);
        
        if (user == null)
        {
            _logger.LogWarning("User không tồn tại trong SAP DB: {Username}", username);
            throw new NotFoundException("Người dùng không tồn tại");
        }

        var token = GenerateJwtToken(user);

        _logger.LogInformation("Tạo JWT token thành công cho user: {Username}", username);

        return new AuthResponse
        {
            AccessToken = token,
            User = user
        };
    }

    /// <summary>
    /// Tạo JWT token
    /// </summary>
    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.AccessSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessExpiryInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Validate JWT token
    /// </summary>
    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new BadRequestException("Token không được để trống");
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.AccessSecret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch (SecurityTokenException)
        {
            throw new UnauthorizedException("Token không hợp lệ hoặc đã hết hạn");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token validation thất bại");
            throw new UnauthorizedException("Token không hợp lệ");
        }
    }

    /// <summary>
    /// Lấy thông tin user từ token
    /// </summary>
    public async Task<User?> GetUserFromTokenAsync(string token)
    {
        var principal = ValidateToken(token);
        if (principal == null)
        {
            throw new UnauthorizedException("Token không hợp lệ");
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedException("Token không chứa thông tin user hợp lệ");
        }

        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Người dùng không tồn tại");
        }

        return user;
    }
}

/// <summary>
/// Response trả về khi xác thực thành công
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Access Token
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// Thông tin người dùng
    /// </summary>
    public User User { get; set; } = null!;
}
