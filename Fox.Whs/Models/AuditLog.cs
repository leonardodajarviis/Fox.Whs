using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fox.Whs.Models;

[Table("FoxWms_AuditLogs")]
public class AuditLog
{
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(128)]
    public string EntityName { get; set; } = null!;

    [Required]
    [MaxLength(16)]
    public string Action { get; set; } = null!;

    public string? KeyValuesJson { get; set; }

    public string? OldValuesJson { get; set; }

    public string? NewValuesJson { get; set; }

    public short? UserId { get; set; }

    [MaxLength(100)]
    public string? Username { get; set; }

    [MaxLength(50)]
    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
