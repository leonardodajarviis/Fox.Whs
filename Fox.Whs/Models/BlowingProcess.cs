using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.SapModels;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Models;

/// <summary>
/// Công đoạn Thổi
/// </summary>
[Table("FoxWms_BlowingProcess")]
public class BlowingProcess
{
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
    /// Tổng sản lượng thổi
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalBlowingOutput { get; set; }

    /// <summary>
    /// Tổng sản lượng tua/chia/tờ
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalRewindingOutput { get; set; }

    /// <summary>
    /// Tổng sản lượng dự trữ
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalReservedOutput { get; set; }

    /// <summary>
    /// Tổng DC công đoạn thổi
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalBlowingLoss { get; set; }

    /// <summary>
    /// Danh sách công nhân thổi
    /// </summary>
    public string? ListOfWorkersText { get; set; }

    public List<BlowingProcessLine> Lines { get; set; } = [];

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
}

/// <summary>
/// Thông tin chi tiết công đoạn thổi sản phẩm
/// </summary>
[Table("FoxWms_BlowingProcessLine")]
public class BlowingProcessLine
{
    [Key]
    public int Id { get; set; }

    public int BlowingProcessId { get; set; }

    public int ProductionOrderId { get; set; }

    [ForeignKey("BlowingProcessId"), JsonIgnore]
    public BlowingProcess? BlowingProcess { get; set; }

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
    /// Máy thổi
    /// </summary>
    public string? BlowingMachine { get; set; }

    [JsonIgnore]
    [ForeignKey("WorkerId")]
    public Employee? Worker { get; set; }

    public int? WorkerId { get; set; }

    /// <summary>
    /// Tên công nhân thổi
    /// </summary>
    [NotMapped]
    public string? WorkerName => Worker?.FirstName;

    /// <summary>
    /// Tốc độ thổi (kg/giờ)
    /// </summary>
    public string? BlowingSpeed { get; set; }

    /// <summary>
    /// Thời gian bắt đầu thổi
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc thổi
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Thời gian dừng máy (phút)
    /// </summary>
    public int StopDurationMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    public string? StopReason { get; set; }

    /// <summary>
    /// Sản lượng thổi (số cuộn)
    /// </summary>
    [Precision(18, 4)]
    public decimal QuantityRolls { get; set; }

    /// <summary>
    /// Sản lượng thổi (số kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Sản lượng tua/chia/tờ (kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal RewindOrSplitWeight { get; set; }

    /// <summary>
    /// Sản lượng dự trữ (kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal ReservedWeight { get; set; }

    /// <summary>
    /// Ngày cần hàng
    /// </summary>
    public DateTime? RequiredDate { get; set; }

    /// <summary>
    /// Xác nhận hoàn thành
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Ngày hoàn thành thực tế
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// Nguyên nhân chậm tiến độ (QLSX ghi)
    /// </summary>
    public string? DelayReason { get; set; }

    /// <summary>
    /// Đổi khổ
    /// </summary>
    [Precision(18, 4)]
    public decimal WidthChange { get; set; }

    /// <summary>
    /// Tráng lòng
    /// </summary>
    [Precision(18, 4)]
    public decimal InnerCoating { get; set; }

    /// <summary>
    /// Cắt via
    /// </summary>
    [Precision(18, 4)]
    public decimal TrimmedEdge { get; set; }

    /// <summary>
    /// Sự cố điện
    /// </summary>
    [Precision(18, 4)]
    public decimal ElectricalIssue { get; set; }

    /// <summary>
    /// DC nguyên liệu (kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal MaterialLossKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC nguyên liệu
    /// </summary>
    public string? MaterialLossReason { get; set; }

    /// <summary>
    /// DC con người (kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal HumanErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC con người
    /// </summary>
    public string? HumanErrorReason { get; set; }

    /// <summary>
    /// DC lỗi máy (kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal MachineErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC lỗi máy
    /// </summary>
    public string? MachineErrorReason { get; set; }

    /// <summary>
    /// DC lỗi khác (kg)
    /// </summary>
    [Precision(18, 4)]
    public decimal OtherErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC lỗi khác
    /// </summary>
    public string? OtherErrorReason { get; set; }

    /// <summary>
    /// Tổng DC
    /// </summary>
    [Precision(18, 4)]
    public decimal TotalLoss { get; set; }

    /// <summary>
    /// Thừa PO
    /// </summary>
    [Precision(18, 4)]
    public decimal ExcessPO { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool SemiProductWarehouseConfirmed { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Tồn kho công đoạn Thổi
    /// </summary>
    [Precision(18, 4)]
    public decimal BlowingStageInventory { get; set; }
}