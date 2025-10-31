using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("OWOR"), ReadOnlyEntity]
public class ProductionOrder
{
    [Column("DocEntry"), Key]
    public int DocEntry { get; set; }

    [Column("DocNum")]
    public int DocNum { get; set; }

    /// <summary>
    /// Mã hàng sản xuất
    /// </summary>
    [Column("ItemCode")]
    public string ItemCode { get; set; } = null!;

    /// <summary>
    /// Mã khách hàng
    /// </summary>
    [Column("CardCode")]
    public string? CardCode { get; set; }

    /// <summary>
    /// Lô sản xuất
    /// </summary>
    [Column("U_LOSX")]
    public string? ProductionBatch { get; set; }

    /// <summary>
    /// Ngày cần hàng (thổi)
    /// </summary>
    [Column("U_THOINCH")]
    public string? DateOfNeedBlowing { get; set; }

    [ForeignKey("ItemCode")]
    public Item? ItemDetail { get; set; }

    [ForeignKey("CardCode")]
    public BusinessPartner? BusinessPartnerDetail { get; set; }
}