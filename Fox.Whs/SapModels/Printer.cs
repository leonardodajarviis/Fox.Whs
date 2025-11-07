using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("@MAYIN"), ReadOnlyEntity]
public class Printer
{
    [Key, Column("Code")]
    public string Code { get; set; } = null!;

    [Column("Name")]
    public string? Name { get; set; } 
}