using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.SapModels;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Models;

/// <summary>
/// Công đoạn Chia
/// </summary>
[Table("FoxWms_SlittingProcess")]
public class SlittingProcess
{
    [Key]
    public int Id { get; set; }

    public int ShiftLeaderId { get; set; }

    [ForeignKey("ShiftLeaderId"), JsonIgnore]
    public Employee? ShiftLeader { get; set; }

    /// <summary>
    /// Bản nháp
    /// </summary>
    public bool IsDraft { get; set; }

    /// <summary>
    /// Tên trưởng ca
    /// </summary>
    [NotMapped]
    public string? ShiftLeaderName => ShiftLeader?.FirstName;

    /// <summary>
    /// Ngày sản xuất
    /// </summary>
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Ca sản xuất
    /// </summary>
    public string ProductionShift { get; set; } = null!;

    /// <summary>
    /// Tổng sản lượng chia
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalSlittingOutput { get; set; }

    /// <summary>
    /// Tổng DC gia công
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalProcessingMold { get; set; }

    /// <summary>
    /// Tổng DC công đoạn Thổi
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalBlowingStageMold { get; set; }

    /// <summary>
    /// Tổng DC công đoạn In
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalPrintingStageMold { get; set; }

    /// <summary>
    /// Tổng DC công đoạn Chia
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalSlittingStageMold { get; set; }

    /// <summary>
    /// Dòng chia
    /// </summary>
    public List<SlittingProcessLine> Lines { get; set; } = [];

    public short CreatorId { get; set; }

    [ForeignKey("CreatorId"), JsonIgnore]
    public User? Creator { get; set; }

    public short? ModifierId { get; set; }

    [ForeignKey("ModifierId"), JsonIgnore]
    public User? Modifier { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    [NotMapped]
    public string? CreatorName => Creator?.FullName;

    [NotMapped]
    public string? ModifierName => Modifier?.FullName;
}

[Table("FoxWms_SlittingProcessLine")]
public class SlittingProcessLine
{
    [Key]
    public int Id { get; set; }
    public int ProductionOrderId { get; set; }

    public int SlittingProcessId { get; set; }

    [ForeignKey("SlittingProcessId"), JsonIgnore]
    public SlittingProcess? SlittingProcess { get; set; }

    /// <summary>
    /// Mã hàng
    /// </summary>
    public string ItemCode { get; set; } = null!;

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
    /// Chủng loại
    /// </summary>
    public string? ProductType { get; set; }

    /// <summary>
    /// Tên chủng loại
    /// </summary>
    public string? ProductTypeName { get; set; }

    /// <summary>
    /// Độ dày / 1 lá
    /// </summary>
    public string? Thickness { get; set; }

    /// <summary>
    /// Khổ màng BTP
    /// </summary>
    public string? SemiProductWidth { get; set; }

    /// <summary>
    /// Tên hình in
    /// </summary>
    public string? PrintPatternName { get; set; }

    /// <summary>
    /// Số màu in
    /// </summary>
    public string? ColorCount { get; set; }

    /// <summary>
    /// Máy chia
    /// </summary>
    public string? SlittingMachine { get; set; } = null!;

    public int? WorkerId { get; set; }

    [ForeignKey("WorkerId"), JsonIgnore]
    public Employee? Worker { get; set; }

    /// <summary>
    /// Tên công nhân in
    /// </summary>
    public string? WorkerName => Worker?.FirstName;

    /// <summary>
    /// Tốc độ chia
    /// </summary>
    [Precision(18, 4)]
    public decimal SlittingSpeed { get; set; }

    /// <summary>
    /// Thời gian bắt đầu chia
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc chia
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Thời gian dừng máy (phút)
    /// </summary>
    [Precision(18, 4)]
    public decimal MachineStopMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    public string? StopReason { get; set; }

    // --- Sản lượng chia ---
    /// <summary>
    /// Số cuộn
    /// </summary>
    [Precision(18, 4)]
    public decimal RollCount { get; set; }

    /// <summary>
    /// Số chiếc
    /// </summary>
    [Precision(18, 4)]
    public decimal PieceCount { get; set; }

    /// <summary>
    /// Số kg
    /// </summary>
    [Precision(18, 4)]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Số thùng
    /// </summary>
    [Precision(18, 4)]
    public decimal BoxCount { get; set; }

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

    // --- DC gia công ---
    /// <summary>
    /// DC gia công (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal ProcessingLossKg { get; set; }

    /// <summary>
    /// DC gia công - Nguyên nhân
    /// </summary>
    public string? ProcessingLossReason { get; set; }

    // --- DC do công đoạn thổi ---
    /// <summary>
    /// DC do công đoạn thổi (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal BlowingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn thổi - Nguyên nhân
    /// </summary>
    public string? BlowingLossReason { get; set; }

    // --- DC do công đoạn in ---
    /// <summary>
    /// DC do công đoạn in (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal PrintingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn in - Nguyên nhân
    /// </summary>
    public string? PrintingLossReason { get; set; }

    /// <summary>
    /// Số máy in
    /// </summary>
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// Cắt via (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal CutViaKg { get; set; }

    // --- DC do công đoạn chia ---
    /// <summary>
    /// DC do công đoạn chia - Con người (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Nguyên nhân con người
    /// </summary>
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Lỗi máy (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Nguyên nhân lỗi máy
    /// </summary>
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Tổng DC (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalLossKg { get; set; }

    // --- Thừa PO ---
    /// <summary>
    /// Thừa PO - BTP In (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal ExcessPOPrinting { get; set; }

    /// <summary>
    /// Thừa PO - TP Chia (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal ExcessPOSlitting { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    [Precision(18, 4)]
    public bool BtpWarehouseConfirmed { get; set; }
}
