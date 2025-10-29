using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("@MAYTHOI"), ReadOnlyEntity]
public class Blower
{
    [Column("Code"), Key]
    public string Code { get; set; } = null!;

    [Column("Name")]
    public string? Name { get; set; }
}