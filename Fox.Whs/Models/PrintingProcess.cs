using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.SapModels;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Models;

/// <summary>
/// Công đoạn In
/// </summary>
[Table("FoxWms_PrintingProcess")]
public class PrintingProcess
{
    [Key]
    public int Id { get; set; }

    public int? ShiftLeaderId { get; set; }

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
    public string? ShiftLeaderName => ShiftLeaderId == null ? ShiftLeaderOriginalName : ShiftLeader?.FullName;

    public string? ShiftLeaderOriginalName { get; set; }

    /// <summary>
    /// Ngày sản xuất
    /// </summary>
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Ca sản xuất
    /// </summary>
    public string ProductionShift { get; set; } = null!;

    /// <summary>
    /// Tổng sản lượng In
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalPrintingOutput { get; set; }

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

    public List<PrintingProcessLine> Lines { get; set; } = [];
    [MaxLength(255)] public string? Notes { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public int Status { get; set; }

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

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}

[Table("FoxWms_PrintingProcessLine")]
public class PrintingProcessLine
{
    [Key]
    public int Id { get; set; }

    public int PrintingProcessId { get; set; }

    [ForeignKey("PrintingProcessId"), JsonIgnore]
    public PrintingProcess? PrintingProcess { get; set; }

    public int ProductionOrderId { get; set; }

    /// <summary>
    /// Mã hàng
    /// </summary>
    public string ItemCode { get; set; } = null!;

    /// <summary>
    /// Tên hàng sản xuất
    /// </summary>
    public string? ItemName { get; set; }

    /// <summary>
    /// Lô sản xuất
    /// </summary>
    public string? ProductionBatch { get; set; }

    [MaxLength(15)]
    public string? CardCode { get; set; }

    /// <summary>
    /// Khách hàng
    /// </summary>
    public string? CustomerName { get; set; }

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
    /// Máy in
    /// </summary>
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// Tên máy in
    /// </summary>
    public string? PrintingMachineName { get; set; }

    public int? WorkerId { get; set; }

    [ForeignKey("WorkerId"), JsonIgnore]
    public Employee? Worker { get; set; }

    /// <summary>
    /// Tên công nhân in
    /// </summary>
    [NotMapped]
    public string? WorkerName => WorkerId == null ? WorkerOriginalName : Worker?.FullName;

    public string? WorkerOriginalName { get; set; }

    /// <summary>
    /// Tốc độ in
    /// </summary>
    public string? PrintingSpeed { get; set; }

    /// <summary>
    /// Thời gian bắt đầu in
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc in
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

    // --- Sản lượng in ---
    /// <summary>
    /// Số cuộn
    /// </summary>
    public int RollCount { get; set; }

    /// <summary>
    /// Số chiếc
    /// </summary>
    public int PieceCount { get; set; }

    /// <summary>
    /// Số kg
    /// </summary>
    [Precision(18, 4)]
    public decimal? QuantityKg { get; set; }

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
    /// Chậm tiến độ - ngày hoàn thành thực tế
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
    /// DC gia công - nguyên nhân
    /// </summary>
    public string? ProcessingLossReason { get; set; }

    // --- DC do công đoạn thổi ---
    /// <summary>
    /// DC do công đoạn thổi (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal BlowingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn thổi - nguyên nhân
    /// </summary>
    public string? BlowingLossReason { get; set; }

    // --- DC do công đoạn in ---
    /// <summary>
    /// Đầu cuộn OPP (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal OppRollHeadKg { get; set; }

    /// <summary>
    /// Đầu cuộn OPP - nguyên nhân
    /// </summary>
    public string? OppRollHeadReason { get; set; }

    /// <summary>
    /// Con người (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// Con người - nguyên nhân
    /// </summary>
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// Lỗi máy (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// Lỗi máy - nguyên nhân
    /// </summary>
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Tổng DC (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalLossKg { get; set; }

    /// <summary>
    /// Thừa PO (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal ExcessPO { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool BtpWarehouseConfirmation { get; set; }

    /// <summary>
    /// Tồn kho ở công đoạn In (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal PrintingStageInventoryKg { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string? Note { get; set; }
}
