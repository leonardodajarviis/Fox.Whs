using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.SapModels;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Models;

/// <summary>
/// Công đoạn Cắt
/// </summary>
[Table("FoxWms_CuttingProcess")]
public class CuttingProcess
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
    /// Tổng sản lượng cắt
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalCuttingOutput { get; set; }

    /// <summary>
    /// Tổng sản lượng gấp xúp
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalFoldedCount { get; set; }

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
    /// Tổng DC công đoạn Cắt
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalCuttingStageMold { get; set; }

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
    [MaxLength(255)] public string? Notes { get; set; }

    [NotMapped]
    public string? CreatorName => Creator?.FullName;
    [NotMapped]
    public string? ModifierName => Modifier?.FullName;
    public List<CuttingProcessLine> Lines { get; set; } = [];

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}

[Table("FoxWms_CuttingProcessLine")]
public class CuttingProcessLine
{
    [Key]
    public int Id { get; set; }

    public int ProductionOrderId { get; set; }

    public int CuttingProcessId { get; set; }

    [ForeignKey("CuttingProcessId"), JsonIgnore]
    public CuttingProcess? CuttingProcess { get; set; }

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
    /// Kích thước
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Số màu in
    /// </summary>
    public string? ColorCount { get; set; }

    /// <summary>
    /// Máy cắt
    /// </summary>
    public string? CuttingMachine { get; set; }

    public int? WorkerId { get; set; }

    [ForeignKey("WorkerId"), JsonIgnore]
    public Employee? Worker { get; set; }

    /// <summary>
    /// Tên công nhân in
    /// </summary>
    public string? WorkerName => Worker?.FirstName;

    /// <summary>
    /// Tốc độ cắt
    /// </summary>
    [Precision(18, 4)]
    public decimal CuttingSpeed { get; set; }

    /// <summary>
    /// Thời gian bắt đầu cắt
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc cắt
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

    /// <summary>
    /// Số chiếc (sản lượng cắt)
    /// </summary>
    [Precision(18, 4)]
    public decimal PieceCount { get; set; }

    /// <summary>
    /// Số kg (sản lượng cắt)
    /// </summary>
    [Precision(18, 4)]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Số bao (sản lượng cắt)
    /// </summary>
    [Precision(18, 4)]
    public decimal BagCount { get; set; }

    /// <summary>
    /// Số lượng gấp xúc
    /// </summary>
    [Precision(18, 4)]
    public decimal FoldedCount { get; set; }

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

    /// <summary>
    /// DC gia công (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal ProcessingLossKg { get; set; }

    /// <summary>
    /// DC gia công - Nguyên nhân
    /// </summary>
    public string? ProcessingLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn thổi (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal BlowingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn thổi - Nguyên nhân
    /// </summary>
    public string? BlowingLossReason { get; set; }

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
    /// Chuyển hàng (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal TransferKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Con người (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Nguyên nhân con người
    /// </summary>
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do cắt dán - Lỗi máy (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Nguyên nhân lỗi máy
    /// </summary>
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Tổng DC (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalLossKg { get; set; }

    /// <summary>
    /// Thừa PO - Cuộn
    /// </summary>
    [Precision(18, 4)]
    public decimal ExcessPOLess5Kg { get; set; }

    /// <summary>
    /// Thừa PO - Cuộn
    /// </summary>
    [Precision(18, 4)]
    public decimal ExcessPOOver5Kg { get; set; }

    /// <summary>
    /// Thừa PO - Hàng cắt (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal ExcessPOCut { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool? BtpWarehouseConfirmed { get; set; }

    /// <summary>
    /// Tồn (Kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal RemainingInventoryKg { get; set; }
}
