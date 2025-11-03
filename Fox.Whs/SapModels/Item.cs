using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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

    [ForeignKey("ProductType"), JsonIgnore]
    public ProductType? ProductTypeInfo { get; set; }

    /// <summary>
    /// Tên chủng loại
    /// </summary>
    [NotMapped]
    public string? ProductTypeName => ProductTypeInfo?.Name;

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

    /// <summary>
    /// Kích thước
    /// </summary>
    [Column("U_KTTPC")]
    public string? Size { get; set; }

    /// <summary>
    /// Tên hình in
    /// </summary>
    [Column("U_THI")]
    public string? PrintPatternName { get; set; }

    /// <summary>
    /// Số màu in
    /// </summary>
    [Column("U_SMI")]
    public string? ColorCount { get; set; }
}