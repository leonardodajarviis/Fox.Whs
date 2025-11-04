using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.SapModels;

namespace Fox.Whs.Models;

/// <summary>
/// Model lưu trữ phiên đăng nhập của User với Refresh Token và Access Token JTI
/// </summary>
[Table("FoxWms_UserSessions")]
public class UserSession
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public short UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    /// <summary>
    /// JTI (JWT ID) của Access Token
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string AccessTokenJti { get; set; } = null!;

    /// <summary>
    /// JTI (JWT ID) của Refresh Token
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string RefreshTokenJti { get; set; } = null!;

    /// <summary>
    /// Refresh Token (mã hóa)
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Ngày hết hạn của phiên
    /// </summary>
    public DateTime RefreshExpiresAt { get; set; }

    /// <summary>
    /// Ngày tạo phiên
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Ngày thu hồi phiên (nếu có)
    /// </summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Lý do thu hồi
    /// </summary>
    [MaxLength(200)]
    public string? RevokedReason { get; set; }

    /// <summary>
    /// IP Address của client
    /// </summary>
    [MaxLength(50)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User Agent của client
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Phiên có còn hoạt động không
    /// </summary>
    [NotMapped]
    public bool IsActive => RevokedAt == null && DateTime.UtcNow < RefreshExpiresAt;

    /// <summary>
    /// Phiên đã hết hạn chưa
    /// </summary>
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= RefreshExpiresAt;
}
