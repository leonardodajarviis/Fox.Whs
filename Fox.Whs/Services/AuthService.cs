using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Options;
using Fox.Whs.Data;
using Fox.Whs.SapModels;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;

namespace Fox.Whs.Services;

public class AuthService
{
    private readonly SapServiceLayerAuthService _sapAuthService;
    private readonly AppDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        SapServiceLayerAuthService sapAuthService,
        AppDbContext sapDbContext,
        IOptions<JwtOptions> jwtOptions,
        IHttpContextAccessor httpContextAccessor)
    {
        _sapAuthService = sapAuthService;
        _dbContext = sapDbContext;
        _jwtOptions = jwtOptions.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Xác thực người dùng và tạo JWT token
    /// </summary>
    public async Task<AuthResponse> AuthenticateAsync(string username, string password)
    {
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
        //     throw new UnauthorizedException("Tên đăng nhập hoặc mật khẩu không đúng");
        // }

        var user = await _dbContext.Users.AsNoTracking()
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new NotFoundException("Người dùng không tồn tại");
        }

        var employee = await _dbContext.Employees.AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == user.Id);
        
        user.EmployeeInfo = employee;

        // Thu hồi TẤT CẢ các phiên cũ của user (logout khỏi tất cả thiết bị khác)
        var oldSessions = await _dbContext.UserSessions
            .Where(s => s.UserId == user.Id && s.RevokedAt == null)
            .ToListAsync();

        foreach (var oldSession in oldSessions)
        {
            oldSession.RevokedAt = DateTime.UtcNow;
            oldSession.RevokedReason = "Logged in from another device";
        }

        // Tạo JTI cho access token và refresh token
        var accessTokenJti = Guid.NewGuid().ToString();
        var refreshTokenJti = Guid.NewGuid().ToString();
        
        var token = GenerateJwtToken(user, accessTokenJti);
        var refreshToken = GenerateRefreshToken();
        
        // Lấy thông tin client
        var httpContext = _httpContextAccessor.HttpContext;
        var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();
        var userAgent = httpContext?.Request.Headers["User-Agent"].ToString();
        
        // Lưu phiên đăng nhập MỚI vào database
        var userSession = new UserSession
        {
            UserId = user.Id,
            AccessTokenJti = accessTokenJti,
            RefreshTokenJti = refreshTokenJti,
            RefreshToken = refreshToken,
            RefreshExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshExpiryInDays),
            CreatedAt = DateTime.UtcNow,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
        
        _dbContext.UserSessions.Add(userSession);
        await _dbContext.SaveChangesAsync();

        return new AuthResponse
        {
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessExpiryInMinutes),
            RefreshTokenExpiresAt = userSession.RefreshExpiresAt,
            AccessToken = token,
            RefreshToken = refreshToken,
            User = user
        };
    }

    /// <summary>
    /// Tạo JWT token
    /// </summary>
    private string GenerateJwtToken(User user, string jti)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.AccessSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            // new Claim(JwtRegisteredClaimNames.Sub, user.Username ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("employeeId", user.EmployeeInfo?.Id.ToString() ?? "no-employee-id"),
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
    /// Tạo refresh token ngẫu nhiên
    /// </summary>
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Làm mới access token bằng refresh token
    /// </summary>
    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new BadRequestException("Refresh token không được để trống");
        }

        // Tìm phiên trong database
        var session = await _dbContext.UserSessions
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken);

        if (session == null)
        {
            throw new UnauthorizedException("Refresh token không hợp lệ");
        }

        // Kiểm tra phiên đã bị thu hồi chưa
        if (session.RevokedAt != null)
        {
            throw new UnauthorizedException("Phiên đã bị thu hồi");
        }

        // Kiểm tra phiên đã hết hạn chưa
        if (session.IsExpired)
        {
            throw new UnauthorizedException("Phiên đã hết hạn");
        }

        // Lấy thông tin user
        var user = await _dbContext.Users.AsNoTracking()
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(u => u.Id == session.UserId);

        if (user == null)
        {
            throw new NotFoundException("Người dùng không tồn tại");
        }

        var employee = await _dbContext.Employees.AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == user.Id);
        
        user.EmployeeInfo = employee;

        // Tạo JTI mới cho access token và refresh token
        var newAccessTokenJti = Guid.NewGuid().ToString();
        var newRefreshTokenJti = Guid.NewGuid().ToString();

        // Tạo access token mới
        var newAccessToken = GenerateJwtToken(user, newAccessTokenJti);
        
        // Tạo refresh token mới
        var newRefreshToken = GenerateRefreshToken();
        
        // Lấy thông tin client
        var httpContext = _httpContextAccessor.HttpContext;
        var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();
        var userAgent = httpContext?.Request.Headers.UserAgent.ToString();

        // Thu hồi phiên cũ
        session.RevokedAt = DateTime.UtcNow;
        session.RevokedReason = "Replaced by new session";
        
        // Tạo phiên mới
        var newSession = new UserSession
        {
            UserId = user.Id,
            AccessTokenJti = newAccessTokenJti,
            RefreshTokenJti = newRefreshTokenJti,
            RefreshToken = newRefreshToken,
            RefreshExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshExpiryInDays),
            CreatedAt = DateTime.UtcNow,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
        
        _dbContext.UserSessions.Add(newSession);
        await _dbContext.SaveChangesAsync();

        return new AuthResponse
        {
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessExpiryInMinutes),
            RefreshTokenExpiresAt = newSession.RefreshExpiresAt,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            User = user
        };
    }

    /// <summary>
    /// Thu hồi phiên (logout)
    /// </summary>
    public async Task RevokeRefreshTokenAsync(string refreshToken, string? reason = null)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        var session = await _dbContext.UserSessions
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken && s.RevokedAt == null);
        
        if (session != null)
        {
            session.RevokedAt = DateTime.UtcNow;
            session.RevokedReason = reason ?? "Logged out";
            await _dbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Thu hồi tất cả phiên của user (logout all devices)
    /// </summary>
    public async Task RevokeAllUserTokensAsync(short userId, string? reason = null)
    {
        var sessions = await _dbContext.UserSessions
            .Where(s => s.UserId == userId && s.RevokedAt == null)
            .ToListAsync();

        foreach (var session in sessions)
        {
            session.RevokedAt = DateTime.UtcNow;
            session.RevokedReason = reason ?? "Logged out from all devices";
        }

        if (sessions.Any())
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Xóa các phiên đã hết hạn
    /// </summary>
    public async Task<int> CleanupExpiredSessionsAsync()
    {
        var expiredSessions = await _dbContext.UserSessions
            .Where(s => s.RefreshExpiresAt < DateTime.UtcNow || s.RevokedAt != null)
            .ToListAsync();

        _dbContext.UserSessions.RemoveRange(expiredSessions);
        await _dbContext.SaveChangesAsync();

        return expiredSessions.Count;
    }

    /// <summary>
    /// Lấy danh sách phiên đang hoạt động của user
    /// </summary>
    public async Task<List<UserSession>> GetUserActiveSessionsAsync(short userId)
    {
        return await _dbContext.UserSessions
            .Where(s => s.UserId == userId && s.RevokedAt == null && s.RefreshExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Validate JWT token và kiểm tra JTI có khớp với session không
    /// </summary>
    public async Task<bool> ValidateAccessTokenJtiAsync(string token)
    {
        try
        {
            var principal = ValidateToken(token);
            if (principal == null)
            {
                return false;
            }

            // Lấy JTI từ token
            var jtiClaim = principal.FindFirst(JwtRegisteredClaimNames.Jti);
            if (jtiClaim == null)
            {
                return false;
            }

            var jti = jtiClaim.Value;

            // Lấy UserId từ token
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !short.TryParse(userIdClaim.Value, out var userId))
            {
                return false;
            }

            // Kiểm tra JTI có tồn tại trong session active của user không
            var sessionExists = await _dbContext.UserSessions
                .AnyAsync(s => 
                    s.UserId == userId && 
                    s.AccessTokenJti == jti && 
                    s.RevokedAt == null && 
                    s.RefreshExpiresAt > DateTime.UtcNow);

            return sessionExists;
        }
        catch
        {
            return false;
        }
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
        catch (Exception)
        {
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

    public DateTime AccessTokenExpiresAt { get; set; }

    public DateTime RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Thông tin người dùng
    /// </summary>
    public User User { get; set; } = null!;
}
