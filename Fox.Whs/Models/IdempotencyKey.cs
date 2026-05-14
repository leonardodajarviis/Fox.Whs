using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fox.Whs.Models;

[Table("FoxWms_IdempotencyKeys")]
public class IdempotencyKey
{
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(36)]
    public string Key { get; set; } = null!;

    public short UserId { get; set; }

    [Required]
    [MaxLength(8)]
    public string Method { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    public string Path { get; set; } = null!;

    [Required]
    [MaxLength(64)]
    public string RequestHash { get; set; } = null!;

    public int ResponseStatus { get; set; }

    public string? ResponseBody { get; set; }

    [MaxLength(128)]
    public string? ResponseContentType { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }
}
