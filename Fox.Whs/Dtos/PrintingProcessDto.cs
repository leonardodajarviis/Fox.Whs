using System.ComponentModel.DataAnnotations;

namespace Fox.Whs.Dtos;

/// <summary>
/// DTO cho việc tạo công đoạn in
/// </summary>
public record CreatePrintingProcessDto
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
    public string? Notes { get; set; }

    /// <summary>
    /// Danh sách chi tiết công đoạn in
    /// </summary>
    public List<CreatePrintingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc cập nhật công đoạn in
/// </summary>
public record UpdatePrintingProcessDto
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
    
    public string? Notes { get; set; }

    /// <summary>
    /// Danh sách chi tiết công đoạn in
    /// </summary>
    public List<UpdatePrintingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc tạo chi tiết công đoạn in
/// </summary>
public record CreatePrintingProcessLineDto
{
    /// <summary>
    /// ID lệnh sản xuất
    /// </summary>
    public int ProductionOrderId { get; set; }

    /// <summary>
    /// Máy in
    /// </summary>
    [StringLength(50)]
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// ID công nhân in
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ in
    /// </summary>
    [StringLength(50)]
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
    [Range(0, double.MaxValue)]
    public decimal MachineStopMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    [StringLength(500)]
    public string? StopReason { get; set; }

    /// <summary>
    /// Số cuộn
    /// </summary>
    [Range(0, int.MaxValue)]
    public int RollCount { get; set; }

    /// <summary>
    /// Số chiếc
    /// </summary>
    [Range(0, int.MaxValue)]
    public int PieceCount { get; set; }

    /// <summary>
    /// Số kg
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal? QuantityKg { get; set; }

    /// <summary>
    /// Xác nhận hoàn thành
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Ngày hoàn thành thực tế
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
    [Range(0, double.MaxValue)]
    public decimal ProcessingLossKg { get; set; }

    /// <summary>
    /// DC gia công - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? ProcessingLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn thổi (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal BlowingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn thổi - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? BlowingLossReason { get; set; }

    /// <summary>
    /// Đầu cuộn OPP (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal OppRollHeadKg { get; set; }

    /// <summary>
    /// Đầu cuộn OPP - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? OppRollHeadReason { get; set; }

    /// <summary>
    /// Con người (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// Con người - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// Lỗi máy (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// Lỗi máy - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Thừa PO
    /// </summary>
    // [Range(0, double.MaxValue)]
    public decimal ExcessPO { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool BtpWarehouseConfirmation { get; set; }

    /// <summary>
    /// Tồn kho ở công đoạn In (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal PrintingStageInventoryKg { get; set; }
}

/// <summary>
/// DTO cho việc cập nhật chi tiết công đoạn in
/// </summary>
public record UpdatePrintingProcessLineDto
{
    /// <summary>
    /// ID chi tiết công đoạn (null nếu là dòng mới)
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// ID lệnh sản xuất
    /// </summary>
    public int ProductionOrderId { get; set; }

    /// <summary>
    /// Chủng loại
    /// </summary>
    [StringLength(100)]
    public string? ProductType { get; set; }

    /// <summary>
    /// Khổ màng BTP
    /// </summary>
    [StringLength(50)]
    public string? SemiProductWidth { get; set; }

    /// <summary>
    /// Tên hình in
    /// </summary>
    [StringLength(200)]
    public string? PrintPatternName { get; set; }

    /// <summary>
    /// Số màu in
    /// </summary>
    [StringLength(50)]
    public string? ColorCount { get; set; }

    /// <summary>
    /// Máy in
    /// </summary>
    [StringLength(50)]
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// ID công nhân in
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ in
    /// </summary>
    [StringLength(50)]
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
    [Range(0, double.MaxValue)]
    public decimal MachineStopMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    [StringLength(500)]
    public string? StopReason { get; set; }

    /// <summary>
    /// Số cuộn
    /// </summary>
    [Range(0, int.MaxValue)]
    public int RollCount { get; set; }

    /// <summary>
    /// Số chiếc
    /// </summary>
    [Range(0, int.MaxValue)]
    public int PieceCount { get; set; }

    /// <summary>
    /// Số kg
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal? QuantityKg { get; set; }

    /// <summary>
    /// Xác nhận hoàn thành
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Ngày hoàn thành thực tế
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
    [Range(0, double.MaxValue)]
    public decimal ProcessingLossKg { get; set; }

    /// <summary>
    /// DC gia công - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? ProcessingLossReason { get; set; }

    /// <summary>
    /// DC do công đoạn thổi (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal BlowingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn thổi - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? BlowingLossReason { get; set; }

    /// <summary>
    /// Đầu cuộn OPP (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal OppRollHeadKg { get; set; }

    /// <summary>
    /// Đầu cuộn OPP - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? OppRollHeadReason { get; set; }

    /// <summary>
    /// Con người (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// Con người - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// Lỗi máy (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// Lỗi máy - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Thừa PO
    /// </summary>
    // [Range(0, double.MaxValue)]
    public decimal ExcessPO { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool BtpWarehouseConfirmation { get; set; }

    /// <summary>
    /// Tồn kho ở công đoạn In (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal PrintingStageInventoryKg { get; set; }
}
