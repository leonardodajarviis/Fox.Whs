using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("OITM"), ReadOnlyEntity]
public class Item
{
    [Column("ItemCode"), Key]
    public string ItemCode { get; set; } = null!;

    [Column("ItemName")]
    public string? ItemName { get; set; }

    [Column("FrgnName")]
    public string? FrgnName { get; set; }

    [Column("ItmsGrpCod")]
    public Int16? ItemGrpCode { get; set; }

    /// <summary>
    /// Chủng loại
    /// </summary>
    [Column("U_CHL")]
    public string? ProductType { get; set; }

    /// <summary>
    /// Độ dày / 1 lá
    /// </summary>
    [Column("U_DD1L")]
    public string? Thickness { get; set; }

    /// <summary>
    /// Khổ màng BTP
    /// </summary>
    [Column("U_KMBTP")]
    public string? SemiProductWidth { get; set; }
}