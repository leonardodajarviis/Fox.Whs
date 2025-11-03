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

    [Column("U_IN")]
    public string? IsPrinting { get; set; }

    [Column("U_INSTATUS")]
    public string? PrintingStatus { get; set; }

    [Column("U_THOI")]
    public string? IsBlowing { get; set; }

    [Column("U_THOISTATUS")]
    public string? BlowingStatus { get; set; }

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
    /// Ngày cần hàng (in)
    /// </summary>
    [Column("U_INNCH")]
    public DateTime? DateOfNeedPrinting { get; set; }

    /// <summary>
    /// Ngày cần hàng (cắt)
    /// </summary>
    [Column("U_CATNCH")]
    public DateTime? DateOfNeedCutting { get; set; }

    /// <summary>
    /// Ngày cần hàng (thổi)
    /// </summary>
    [Column("U_THOINCH")]
    public DateTime? DateOfNeedBlowing { get; set; }

    [ForeignKey("ItemCode")]
    public Item? ItemDetail { get; set; }

    [ForeignKey("CardCode")]
    public BusinessPartner? BusinessPartnerDetail { get; set; }
}