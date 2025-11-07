using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fox.Whs.SapModels;

[Table("@MAYCHIA")]
public class SlittingMachine
{
    [Key, Column("Code")]
    public string Code { get; set; } = null!;

    [Column("Name")]
    public string? Name { get; set; } 
}