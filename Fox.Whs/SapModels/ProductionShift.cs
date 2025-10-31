using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace BackEnd_LHC.SapModels;

[Table("@CASX"), ReadOnlyEntity]
public class ProductionShift
{
    [Key]
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}