using System.ComponentModel.DataAnnotations;

namespace Fox.Whs.SapModels;

public class Printer
{
    [Key]
    public string Code { get; set; } = null!;

    public string? Name { get; set; } 
}