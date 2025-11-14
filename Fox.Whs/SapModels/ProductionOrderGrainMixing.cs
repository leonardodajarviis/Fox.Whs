using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("@LPH_H"), ReadOnlyEntity]
public class ProductionOrderGrainMixing
{
    [Column("DocEntry"), Key]
    public int DocEntry { get; set; }

    /// <summary>
    /// Mã khách hàng
    /// </summary>
    [Column("U_KH")]
    public string? CardCode { get; set; }

    [Column("U_PHASTATUS")]
    public string? Status { get; set; }

    /// <summary>
    /// Lô sản xuất
    /// </summary>
    [Column("U_LSX")]
    public string? ProductionBatch { get; set; }

    /// <summary>
    /// Ngày cần hàng
    /// </summary>
    [Column("U_NCH")]
    public DateTime? DateOfNeed { get; set; }
}