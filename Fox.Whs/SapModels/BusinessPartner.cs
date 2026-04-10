using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("OCRD"), ReadOnlyEntity]
public class BusinessPartner
{
    [Column("CardCode"), Key]
    public string CardCode { get; set; } = null!;

    [Column("CardFName")]
    public string? CardName { get; set; }
}