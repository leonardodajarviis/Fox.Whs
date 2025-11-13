using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.SapModels;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Models;

/// <summary>
/// Công đoạn Pha hạt (Thổi)
/// </summary>
[Table("FoxWms_GrainMixingBlowingProcess")]
public class GrainMixingBlowingProcess
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Bản nháp
    /// </summary>
    public bool IsDraft { get; set; }

    /// <summary>
    /// Ngày sản xuất (ngày pha)
    /// </summary>
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Máy thổi
    /// </summary>
    public string? BlowingMachine { get; set; }

    public short CreatorId { get; set; }

    [ForeignKey("CreatorId"), JsonIgnore]
    public User? Creator { get; set; }

    public short? ModifierId { get; set; }

    [ForeignKey("ModifierId"), JsonIgnore]
    public User? Modifier { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }

    [NotMapped]
    public string? CreatorName => Creator?.FullName;
    [NotMapped]
    public string? ModifierName => Modifier?.FullName;
    public List<GrainMixingBlowingProcessLine> Lines { get; set; } = [];

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}

[Table("FoxWms_GrainMixingBlowingProcessLine")]
public class GrainMixingBlowingProcessLine
{
    [Key]
    public int Id { get; set; }

    public int GrainMixingBlowingProcessId { get; set; }

    [ForeignKey("GrainMixingBlowingProcessId"), JsonIgnore]
    public GrainMixingBlowingProcess? GrainMixingBlowingProcess { get; set; }

    /// <summary>
    /// Lô sản xuất
    /// </summary>
    public string? ProductionBatch { get; set; }

    [MaxLength(15)]
    public string? CardCode { get; set; }

    [ForeignKey("CardCode"), JsonIgnore]
    public BusinessPartner? BusinessPartner { get; set; }

    /// <summary>
    /// Khách hàng
    /// </summary>
    [NotMapped]
    public string? CustomerName => BusinessPartner?.CardName;

    /// <summary>
    /// Số phiếu lĩnh vật tư
    /// </summary>
    public string? MaterialIssueVoucherNo { get; set; }

    /// <summary>
    /// Chủng loại pha (HD, PE, Màng co, Màng chít, PP...)
    /// </summary>
    public string? MixtureType { get; set; }

    /// <summary>
    /// Quy cách diễn giải
    /// </summary>
    public string? Specification { get; set; }

    public int? WorkerId { get; set; }

    [ForeignKey("WorkerId"), JsonIgnore]
    public Employee? Worker { get; set; }

    /// <summary>
    /// Tên công nhân in
    /// </summary>
    public string? WorkerName => Worker?.FirstName;

    /// <summary>
    /// Tên máy pha
    /// </summary>
    public string? MachineName { get; set; }

    /// <summary>
    /// Thời gian bắt đầu pha
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc pha
    /// </summary>
    public DateTime? EndTime { get; set; }

    // ------------------ PP ------------------

    /// <summary>
    /// Hạt PP trơn
    /// </summary>
    [Precision(18, 6)]
    public decimal PpTron { get; set; }

    /// <summary>
    /// Hạt HD nhớt
    /// </summary>
    [Precision(18, 6)]
    public decimal PpHdNhot { get; set; }

    /// <summary>
    /// Hạt LDPE
    /// </summary>
    [Precision(18, 6)]
    public decimal PpLdpe { get; set; }

    /// <summary>
    /// Hạt DC
    /// </summary>
    [Precision(18, 6)]
    public decimal PpDc { get; set; }

    /// <summary>
    /// Phụ gia
    /// </summary>
    [Precision(18, 6)]
    public decimal PpAdditive { get; set; }

    /// <summary>
    /// Hạt màu
    /// </summary>
    [Precision(18, 6)]
    public decimal PpColor { get; set; }

    /// <summary>
    /// Hạt khác
    /// </summary>
    [Precision(18, 6)]
    public decimal PpOther { get; set; }

    // ------------------ HD ------------------

    /// <summary>
    /// Hạt LLDPE 2320
    /// </summary>
    [Precision(18, 6)]
    public decimal HdLldpe2320 { get; set; }

    /// <summary>
    /// Hạt tái chế
    /// </summary>
    [Precision(18, 6)]
    public decimal HdRecycled { get; set; }

    /// <summary>
    /// Hạt Talcol
    /// </summary>
    [Precision(18, 6)]
    public decimal HdTalcol { get; set; }

    /// <summary>
    /// Hạt DC
    /// </summary>
    [Precision(18, 6)]
    public decimal HdDc { get; set; }

    /// <summary>
    /// Hạt màu
    /// </summary>
    [Precision(18, 6)]
    public decimal HdColor { get; set; }

    /// <summary>
    /// Hạt khác
    /// </summary>
    [Precision(18, 6)]
    public decimal HdOther { get; set; }

    // ------------------ PE ------------------

    /// <summary>
    /// Phụ gia
    /// </summary>
    [Precision(18, 6)]
    public decimal PeAdditive { get; set; }

    /// <summary>
    /// Talcol
    /// </summary>
    [Precision(18, 6)]
    public decimal PeTalcol { get; set; }

    /// <summary>
    /// Hạt màu
    /// </summary>
    [Precision(18, 6)]
    public decimal PeColor { get; set; }

    /// <summary>
    /// Hạt khác
    /// </summary>
    [Precision(18, 6)]
    public decimal PeOther { get; set; }

    /// <summary>
    /// Hạt LDPE
    /// </summary>
    [Precision(18, 6)]
    public decimal PeLdpe { get; set; }

    /// <summary>
    /// Hạt LLDPE
    /// </summary>
    [Precision(18, 6)]
    public decimal PeLldpe { get; set; }

    /// <summary>
    /// Hạt tái chế
    /// </summary>
    [Precision(18, 6)]
    public decimal PeRecycled { get; set; }

    // ------------------ Màng co ------------------

    /// <summary>
    /// Tăng R8707
    /// </summary>
    [Precision(18, 6)]
    public decimal ShrinkRe707 { get; set; }

    /// <summary>
    /// Tăng Slip
    /// </summary>
    [Precision(18, 6)]
    public decimal ShrinkSlip { get; set; }

    /// <summary>
    /// Tăng tĩnh điện
    /// </summary>
    [Precision(18, 6)]
    public decimal ShrinkStatic { get; set; }

    /// <summary>
    /// Hạt DC
    /// </summary>
    [Precision(18, 6)]
    public decimal ShrinkDc { get; set; }

    /// <summary>
    /// Talcol
    /// </summary>
    [Precision(18, 6)]
    public decimal ShrinkTalcol { get; set; }

    /// <summary>
    /// Hạt khác
    /// </summary>
    [Precision(18, 6)]
    public decimal ShrinkOther { get; set; }

    // ------------------ Màng chít ------------------

    /// <summary>
    /// Hạt tái chế Ca
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapRecycledCa { get; set; }

    /// <summary>
    /// Hạt tái chế Cb
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapRecycledCb { get; set; }

    /// <summary>
    /// Keo
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapGlue { get; set; }

    /// <summary>
    /// Hạt màu
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapColor { get; set; }

    /// <summary>
    /// Hạt DC
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapDc { get; set; }

    /// <summary>
    /// Hạt LDPE
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapLdpe { get; set; }

    /// <summary>
    /// Hạt LLDPE
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapLldpe { get; set; }

    /// <summary>
    /// Hạt Slip
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapSlip { get; set; }

    /// <summary>
    /// Phụ gia
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapAdditive { get; set; }

    /// <summary>
    /// Hạt khác
    /// </summary>
    [Precision(18, 6)]
    public decimal WrapOther { get; set; }

    // ------------------ EVA ------------------

    /// <summary>
    /// POP 3070
    /// </summary>
    [Precision(18, 6)]
    public decimal EvaPop3070 { get; set; }

    /// <summary>
    /// Hạt LDPE
    /// </summary>
    [Precision(18, 6)]
    public decimal EvaLdpe { get; set; }

    /// <summary>
    /// Hạt DC
    /// </summary>
    [Precision(18, 6)]
    public decimal EvaDc { get; set; }

    /// <summary>
    /// Hạt Talcol
    /// </summary>
    [Precision(18, 6)]
    public decimal EvaTalcol { get; set; }

    /// <summary>
    /// Slip
    /// </summary>
    [Precision(18, 6)]
    public decimal EvaSlip { get; set; }

    /// <summary>
    /// Trợ tĩnh chống dính
    /// </summary>
    [Precision(18, 6)]
    public decimal EvaStaticAdditive { get; set; }

    /// <summary>
    /// Hạt khác
    /// </summary>
    [Precision(18, 6)]
    public decimal EvaOther { get; set; }

    /// <summary>
    /// Sản lượng pha (Kg)
    /// </summary>
    [Precision(18, 6)]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Ngày cần hàng
    /// </summary>
    public DateTime? RequiredDate { get; set; }

    /// <summary>
    /// Xác nhận hoàn thành
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Ngày hoàn thành thực tế (QLSX)
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// Nguyên nhân chậm tiến độ
    /// </summary>
    public string? DelayReason { get; set; }
}