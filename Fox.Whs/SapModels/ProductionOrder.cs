using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.SapModels;

[Table("OWOR"), ReadOnlyEntity]
public class ProductionOrder
{
    [Column("DocEntry"), Key] public int DocEntry { get; set; }

    [Column("DocNum")] public int DocNum { get; set; }

    /// <summary>
    /// Mã hàng sản xuất
    /// </summary>
    [Column("ItemCode")]
    public string ItemCode { get; set; } = null!;

    /// <summary>
    /// Tên hàng sản xuất
    /// </summary>
    [Column("ProdName")]
    public string? ProdName {get; set;}

    [Column("U_IN")] public string? IsPrinting { get; set; }

    [Column("U_INSTATUS")] public string? PrintingStatus { get; set; }

    [Column("U_THOI")] public string? IsBlowing { get; set; }

    [Column("U_THOISTATUS")] public string? BlowingStatus { get; set; }

    [Column("U_TUA")] public string? IsRewinding { get; set; }

    [Column("U_TUASTATUS")] public string? RewindingStatus { get; set; }

    [Column("U_CAT")] public string? IsCutting { get; set; }

    [Column("U_CATSTATUS")] public string? CuttingStatus { get; set; }

    [Column("U_CHIA")] public string? IsSlitting { get; set; }

    [Column("U_CHIASTATUS")] public string? SlittingStatus { get; set; }

    /// <summary>
    /// Mã khách hàng
    /// </summary>
    [Column("CardCode")]
    public string? CardCode { get; set; }

    [Column("Status")] public string? Status { get; set; }

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

    /// <summary>
    /// Ngày cần hàng (tua)
    /// </summary>
    [Column("U_TUANCH")]
    public DateTime? DateOfNeedRewinding { get; set; }

    /// <summary>
    /// Ngày cần hàng (chia)
    /// </summary>
    [Column("U_CHIANCH")]
    public DateTime? DateOfNeedSlitting { get; set; }

    /// <summary>
    /// Số lượng còn lại
    /// </summary>
    [Precision(18, 2)]
    [NotMapped]
    public decimal RemainingQuantity { get; set; }

    [Column("U_THOIL")]
    [Precision(18, 2)]
    public decimal? BlowingQuantity { get; set; }

    // [Column("U_CATSL")] public decimal? CuttingQuantity { get; set; }

    [Column("U_CHIASL")]
    [Precision(18, 2)]
    public decimal? SlittingQuantity { get; set; }

    [Column("U_INSL")]
    [Precision(18, 2)]
    public decimal? PrintingQuantity { get; set; }

    [Column("U_TUASL")]
    [Precision(18, 2)]
    public decimal? RewindingQuantity { get; set; }


    #region Số lượng tối thiểu/tối đa của in, chia, cắt, thổi, tua

    [NotMapped] public decimal? QuantityProduced { get; set; }
    [NotMapped] public decimal? QuantityPcs { get; set; }

    [Column("U_THOISLMAX")]
    [Precision(18, 2)]
    public decimal? BlowingMaxQuantity { get; set; }

    [Column("U_THOISLMIN")]
    [Precision(18, 2)]
    public decimal? BlowingMinQuantity { get; set; }

    [Column("U_INSLMIX")]
    [Precision(18, 2)]
    public decimal? PrintingMaxQuantity { get; set; }

    [Column("U_INSLMIN")]
    [Precision(18, 2)]
    public decimal? PrinringMinQuantity { get; set; }

    [Column("U_CATSLMAX")]
    [Precision(18, 2)]
    public decimal? CuttingMaxQuantity { get; set; }

    [Column("U_CATSLMIN")]
    [Precision(18, 2)]
    public decimal? CuttingMinQuantity { get; set; }

    [Column("U_CATSC")]
    [Precision(18, 2)]
    public int? CuttingQuantity { get; set; }

    [Column("U_CHIASLMAX")]
    [Precision(18, 2)]
    public decimal? SlittingMaxQuantity { get; set; }

    [Column("U_CHIASLMIN")]
    [Precision(18, 2)]
    public decimal? SlittingMinQuantity { get; set; }

    [Column("U_TUASLMAX")]
    [Precision(18, 2)]
    public decimal? RewindingMaxQuantity { get; set; }

    [Column("U_TUASLMIN")]
    [Precision(18, 2)]
    public decimal? RewindingMinQuantity { get; set; }

    #endregion


    public decimal Quantity()
    {
        if (IsBlowing == "Y")
        {
            return BlowingQuantity ?? 0;
        }

        if (IsCutting == "Y")
        {
            return CuttingQuantity ?? 0;
        }

        if (IsSlitting == "Y")
        {
            return SlittingQuantity ?? 0;
        }

        if (IsPrinting == "Y")
        {
            return PrintingQuantity ?? 0;
        }

        if (IsRewinding == "Y")
        {
            return RewindingQuantity ?? 0;
        }

        return 0;
    }


    [ForeignKey("ItemCode")] public Item? ItemDetail { get; set; }

    [ForeignKey("CardCode")] public BusinessPartner? BusinessPartnerDetail { get; set; }

    public string? CustomerName => BusinessPartnerDetail?.CardName;
}