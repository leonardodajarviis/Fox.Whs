using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.SapModels;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Models;

/// <summary>
/// Công đoạn Tua
/// </summary>
[Table("FoxWms_RewindingProcess")]
public class RewindingProcess
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
    public string? ShiftLeaderName => ShiftLeader?.FullName;

    /// <summary>
    /// Ngày sản xuất
    /// </summary>
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Ca sản xuất
    /// </summary>
    public string ProductionShift { get; set; } = null!;

    /// <summary>
    /// Tổng sản lượng tua
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalRewindingOutput { get; set; }

    /// <summary>
    /// Tổng DC công đoạn Thổi
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalBlowingStageMold { get; set; }

    /// <summary>
    /// Tổng DC công đoạn Tua
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalRewindingStageMold { get; set; }

    /// <summary>
    /// Danh sách chi tiết công đoạn tua
    /// </summary>
    public List<RewindingProcessLine> Lines { get; set; } = [];

    /// <summary>
    /// Trạng thái
    /// </summary>
    public int Status { get; set; }

    public short CreatorId { get; set; }

    [ForeignKey("CreatorId"), JsonIgnore]
    public User? Creator { get; set; }
    [MaxLength(255)] public string? Notes { get; set; }

    public short? ModifierId { get; set; }

    [ForeignKey("ModifierId"), JsonIgnore]
    public User? Modifier { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    [NotMapped]
    public string? CreatorName => Creator?.FullName;
    [NotMapped]
    public string? ModifierName => Modifier?.FullName;
}

[Table("FoxWms_RewindingProcessLine")]
public class RewindingProcessLine
{
    [Key]
    public int Id { get; set; }
    public int ProductionOrderId { get; set; }

    public int RewindingProcessId { get; set; }

    [ForeignKey("RewindingProcessId"), JsonIgnore]
    public RewindingProcess? RewindingProcess { get; set; }

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
    /// Máy tua
    /// </summary>
    public string? RewindingMachine { get; set; }

    /// <summary>
    /// Tên máy tua
    /// </summary>
    public string? RewindingMachineName { get; set; }

    public int? WorkerId { get; set; }

    [ForeignKey("WorkerId"), JsonIgnore]
    public Employee? Worker { get; set; }

    /// <summary>
    /// Tên công nhân in
    /// </summary>
    public string? WorkerName => Worker?.FullName;

    /// <summary>
    /// Tốc độ tua
    /// </summary>
    [Precision(18, 4)]
    public decimal RewindingSpeed { get; set; }

    /// <summary>
    /// Thời gian bắt đầu tua
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc tua
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

    // --- Sản lượng tua ---
    /// <summary>
    /// Số cuộn
    /// </summary>
    [Precision(18, 4)]
    public decimal RollCount { get; set; }

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

    // --- DC do công đoạn tua ---
    /// <summary>
    /// DC do công đoạn tua - Con người (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Nguyên nhân con người
    /// </summary>
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Lỗi máy (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Nguyên nhân lỗi máy
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
    [Precision(18, 4)]
    public bool BtpWarehouseConfirmed { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string? Note { get; set; }
}
