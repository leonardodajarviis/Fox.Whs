using System.ComponentModel.DataAnnotations;

namespace Fox.Whs.Dtos;

/// <summary>
/// DTO cho việc tạo công đoạn chia
/// </summary>
public record CreateSlittingProcessDto
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
    /// Danh sách chi tiết công đoạn chia
    /// </summary>
    public List<CreateSlittingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc cập nhật công đoạn chia
/// </summary>
public record UpdateSlittingProcessDto
{
    /// <summary>
    /// Ngày sản xuất
    /// </summary>
    [Required(ErrorMessage = "Ngày sản xuất là bắt buộc")]
    public DateTime ProductionDate { get; set; }

    public int ShiftLeaderId { get; set; }

    public bool IsDraft { get; set; }

    /// <summary>
    /// Ca sản xuất
    /// </summary>
    [Required(ErrorMessage = "Ca sản xuất là bắt buộc")]
    [StringLength(50)]
    public string ProductionShift { get; set; } = null!;

    /// <summary>
    /// Danh sách chi tiết công đoạn chia
    /// </summary>
    public List<UpdateSlittingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc tạo chi tiết công đoạn chia
/// </summary>
public record CreateSlittingProcessLineDto
{
    /// <summary>
    /// ID lệnh sản xuất
    /// </summary>
    public int ProductionOrderId { get; set; }

    /// <summary>
    /// Máy chia
    /// </summary>
    [StringLength(50)]
    public string? SlittingMachine { get; set; }

    /// <summary>
    /// ID công nhân chia
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ chia
    /// </summary>
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
    /// Số chiếc
    /// </summary>
    public decimal PieceCount { get; set; }

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
    [StringLength(500)]
    public string? DelayReason { get; set; }

    /// <summary>
    /// DC gia công (Kg)
    /// </summary>
    public decimal ProcessingLossKg { get; set; }

    /// <summary>
    /// DC gia công - Nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? ProcessingLossReason { get; set; }

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
    /// DC do công đoạn in (Kg)
    /// </summary>
    public decimal PrintingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn in - Nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? PrintingLossReason { get; set; }

    /// <summary>
    /// Số máy in
    /// </summary>
    [StringLength(50)]
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// Cắt via (Kg)
    /// </summary>
    public decimal CutViaKg { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Con người (Kg)
    /// </summary>
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Nguyên nhân con người
    /// </summary>
    [StringLength(500)]
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Lỗi máy (Kg)
    /// </summary>
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Nguyên nhân lỗi máy
    /// </summary>
    [StringLength(500)]
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Thừa PO - BTP In (Kg)
    /// </summary>
    public decimal ExcessPOPrinting { get; set; }

    /// <summary>
    /// Thừa PO - TP Chia (Kg)
    /// </summary>
    public decimal ExcessPOSlitting { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool BtpWarehouseConfirmed { get; set; }
}

/// <summary>
/// DTO cho việc cập nhật chi tiết công đoạn chia
/// </summary>
public record UpdateSlittingProcessLineDto
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
    /// Máy chia
    /// </summary>
    [StringLength(50)]
    public string? SlittingMachine { get; set; }

    /// <summary>
    /// ID công nhân chia
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ chia
    /// </summary>
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
    /// Số chiếc
    /// </summary>
    public decimal PieceCount { get; set; }

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
    [StringLength(500)]
    public string? DelayReason { get; set; }

    /// <summary>
    /// DC gia công (Kg)
    /// </summary>
    public decimal ProcessingLossKg { get; set; }

    /// <summary>
    /// DC gia công - Nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? ProcessingLossReason { get; set; }

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
    /// DC do công đoạn in (Kg)
    /// </summary>
    public decimal PrintingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn in - Nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? PrintingLossReason { get; set; }

    /// <summary>
    /// Số máy in
    /// </summary>
    [StringLength(50)]
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// Cắt via (Kg)
    /// </summary>
    public decimal CutViaKg { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Con người (Kg)
    /// </summary>
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Nguyên nhân con người
    /// </summary>
    [StringLength(500)]
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Lỗi máy (Kg)
    /// </summary>
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn chia - Nguyên nhân lỗi máy
    /// </summary>
    [StringLength(500)]
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Thừa PO - BTP In (Kg)
    /// </summary>
    public decimal ExcessPOPrinting { get; set; }

    /// <summary>
    /// Thừa PO - TP Chia (Kg)
    /// </summary>
    public decimal ExcessPOSlitting { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool BtpWarehouseConfirmed { get; set; }
}
