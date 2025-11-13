using System.ComponentModel.DataAnnotations;

namespace Fox.Whs.Dtos;

/// <summary>
/// DTO cho việc tạo công đoạn tua
/// </summary>
public record CreateRewindingProcessDto
{
    /// <summary>
    /// Bản nháp
    /// </summary>
    public bool IsDraft { get; set; }

    public int? ShiftLeaderId { get; set; }

    /// <summary>
    /// Ngày sản xuất
    /// </summary>
    [Required(ErrorMessage = "Ngày sản xuất là bắt buộc")]
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Ca sản xuất
    /// </summary>
    [Required(ErrorMessage = "Ca sản xuất là bắt buộc")]
    [StringLength(50)]
    public string ProductionShift { get; set; } = null!;

    /// <summary>
    /// Danh sách chi tiết công đoạn tua
    /// </summary>
    public List<CreateRewindingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc cập nhật công đoạn tua
/// </summary>
public record UpdateRewindingProcessDto
{
    public int ShiftLeaderId { get; set; }

    /// <summary>
    /// Ngày sản xuất
    /// </summary>
    [Required(ErrorMessage = "Ngày sản xuất là bắt buộc")]
    public DateTime ProductionDate { get; set; }

    public bool IsDraft { get; set; }

    /// <summary>
    /// Ca sản xuất
    /// </summary>
    [Required(ErrorMessage = "Ca sản xuất là bắt buộc")]
    [StringLength(50)]
    public string ProductionShift { get; set; } = null!;

    /// <summary>
    /// Danh sách chi tiết công đoạn tua
    /// </summary>
    public List<UpdateRewindingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc tạo chi tiết công đoạn tua
/// </summary>
public record CreateRewindingProcessLineDto
{
    /// <summary>
    /// ID lệnh sản xuất
    /// </summary>
    public int ProductionOrderId { get; set; }

    /// <summary>
    /// Máy tua
    /// </summary>
    [StringLength(50)]
    public string? RewindingMachine { get; set; }

    /// <summary>
    /// ID công nhân tua
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ tua
    /// </summary>
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
    public decimal MachineStopMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    [StringLength(500)]
    public string? StopReason { get; set; }

    /// <summary>
    /// Số cuộn
    /// </summary>
    public decimal RollCount { get; set; }

    /// <summary>
    /// Số kg
    /// </summary>
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Số thùng
    /// </summary>
    public decimal BoxCount { get; set; }

    /// <summary>
    /// Xác nhận hoàn thành
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Ngày hoàn thành thực tế (QLSX)
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// Nguyên nhân chậm tiến độ
    /// </summary>
    [StringLength(500)]
    public string? DelayReason { get; set; }

    /// <summary>
    /// DC do công đoạn thổi (Kg)
    /// </summary>
    public decimal BlowingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn thổi - Nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? BlowingLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Con người (Kg)
    /// </summary>
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Nguyên nhân con người
    /// </summary>
    [StringLength(500)]
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Lỗi máy (Kg)
    /// </summary>
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Nguyên nhân lỗi máy
    /// </summary>
    [StringLength(500)]
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Thừa PO (Kg)
    /// </summary>
    public decimal ExcessPO { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool BtpWarehouseConfirmed { get; set; }
}

/// <summary>
/// DTO cho việc cập nhật chi tiết công đoạn tua
/// </summary>
public record UpdateRewindingProcessLineDto
{
    /// <summary>
    /// ID của line (null nếu là line mới)
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// ID lệnh sản xuất
    /// </summary>
    public int ProductionOrderId { get; set; }

    /// <summary>
    /// Máy tua
    /// </summary>
    [StringLength(50)]
    public string? RewindingMachine { get; set; }

    /// <summary>
    /// ID công nhân tua
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ tua
    /// </summary>
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
    public decimal MachineStopMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    [StringLength(500)]
    public string? StopReason { get; set; }

    /// <summary>
    /// Số cuộn
    /// </summary>
    public decimal RollCount { get; set; }

    /// <summary>
    /// Số kg
    /// </summary>
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Số thùng
    /// </summary>
    public decimal BoxCount { get; set; }

    /// <summary>
    /// Xác nhận hoàn thành
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Ngày hoàn thành thực tế (QLSX)
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// Nguyên nhân chậm tiến độ
    /// </summary>
    [StringLength(500)]
    public string? DelayReason { get; set; }

    /// <summary>
    /// DC do công đoạn thổi (Kg)
    /// </summary>
    public decimal BlowingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn thổi - Nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? BlowingLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Con người (Kg)
    /// </summary>
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Nguyên nhân con người
    /// </summary>
    [StringLength(500)]
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Lỗi máy (Kg)
    /// </summary>
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn tua - Nguyên nhân lỗi máy
    /// </summary>
    [StringLength(500)]
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Thừa PO (Kg)
    /// </summary>
    public decimal ExcessPO { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool BtpWarehouseConfirmed { get; set; }
}
