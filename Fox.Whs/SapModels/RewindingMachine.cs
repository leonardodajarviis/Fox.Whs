using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("@MAYTUA"), ReadOnlyEntity]
public class RewindingMachine
{
    [Key, Column("Code")]
    public string Code { get; set; } = null!;

    [Column("Name")]
    public string? Name { get; set; } 
}