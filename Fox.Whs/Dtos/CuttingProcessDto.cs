using System.ComponentModel.DataAnnotations;

namespace Fox.Whs.Dtos;

/// <summary>
/// DTO cho việc tạo công đoạn cắt
/// </summary>
public record CreateCuttingProcessDto
{
    /// <summary>
    /// Bản nháp
    /// </summary>
    public bool IsDraft { get; set; }

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

    public int? ShiftLeaderId { get; set; }

    /// <summary>
    /// Danh sách chi tiết công đoạn cắt
    /// </summary>
    public List<CreateCuttingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc cập nhật công đoạn cắt
/// </summary>
public record UpdateCuttingProcessDto
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
    /// Danh sách chi tiết công đoạn cắt
    /// </summary>
    public List<UpdateCuttingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc tạo chi tiết công đoạn cắt
/// </summary>
public record CreateCuttingProcessLineDto
{
    /// <summary>
    /// ID lệnh sản xuất
    /// </summary>
    public int ProductionOrderId { get; set; }

    /// <summary>
    /// Kích thước
    /// </summary>
    [StringLength(100)]
    public string? Size { get; set; }

    /// <summary>
    /// Số màu in
    /// </summary>
    [StringLength(50)]
    public string? ColorCount { get; set; }

    /// <summary>
    /// Máy cắt
    /// </summary>
    [StringLength(50)]
    public string? CuttingMachine { get; set; }

    /// <summary>
    /// ID công nhân cắt
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ cắt
    /// </summary>
    [Range(0, double.MaxValue)]
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
    [Range(0, double.MaxValue)]
    public decimal MachineStopMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    [StringLength(500)]
    public string? StopReason { get; set; }

    /// <summary>
    /// Số chiếc (sản lượng cắt)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal PieceCount { get; set; }

    /// <summary>
    /// Số kg (sản lượng cắt)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Số bao (sản lượng cắt)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal BagCount { get; set; }

    /// <summary>
    /// Số lượng gấp xúc
    /// </summary>
    [Range(0, double.MaxValue)]
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
    /// DC do công đoạn in (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal PrintingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn in - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? PrintingLossReason { get; set; }

    /// <summary>
    /// Số máy in
    /// </summary>
    [StringLength(50)]
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// Chuyển hàng (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal TransferKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Con người (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Nguyên nhân con người
    /// </summary>
    [StringLength(500)]
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do cắt dán - Lỗi máy (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Nguyên nhân lỗi máy
    /// </summary>
    [StringLength(500)]
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Thừa PO - Cuộn dưới 5kg
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ExcessPOLess5Kg { get; set; }

    /// <summary>
    /// Thừa PO - Cuộn trên 5kg
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ExcessPOOver5Kg { get; set; }

    /// <summary>
    /// Thừa PO - Hàng cắt (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ExcessPOCut { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool? BtpWarehouseConfirmed { get; set; }

    /// <summary>
    /// Tồn (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal RemainingInventoryKg { get; set; }
}

/// <summary>
/// DTO cho việc cập nhật chi tiết công đoạn cắt
/// </summary>
public record UpdateCuttingProcessLineDto
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
    /// Độ dày / 1 lá
    /// </summary>
    [StringLength(50)]
    public string? Thickness { get; set; }

    /// <summary>
    /// Khổ màng BTP
    /// </summary>
    [StringLength(50)]
    public string? SemiProductWidth { get; set; }

    /// <summary>
    /// Máy cắt
    /// </summary>
    [StringLength(50)]
    public string? CuttingMachine { get; set; }

    /// <summary>
    /// ID công nhân cắt
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ cắt
    /// </summary>
    [Range(0, double.MaxValue)]
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
    [Range(0, double.MaxValue)]
    public decimal MachineStopMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    [StringLength(500)]
    public string? StopReason { get; set; }

    /// <summary>
    /// Số chiếc (sản lượng cắt)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal PieceCount { get; set; }

    /// <summary>
    /// Số kg (sản lượng cắt)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Số bao (sản lượng cắt)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal BagCount { get; set; }

    /// <summary>
    /// Số lượng gấp xúc
    /// </summary>
    [Range(0, double.MaxValue)]
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
    /// DC do công đoạn in (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal PrintingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn in - nguyên nhân
    /// </summary>
    [StringLength(500)]
    public string? PrintingLossReason { get; set; }

    /// <summary>
    /// Số máy in
    /// </summary>
    [StringLength(50)]
    public string? PrintingMachine { get; set; }

    /// <summary>
    /// Chuyển hàng (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal TransferKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Con người (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal HumanLossKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Nguyên nhân con người
    /// </summary>
    [StringLength(500)]
    public string? HumanLossReason { get; set; }

    /// <summary>
    /// DC do cắt dán - Lỗi máy (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MachineLossKg { get; set; }

    /// <summary>
    /// DC do cắt dán - Nguyên nhân lỗi máy
    /// </summary>
    [StringLength(500)]
    public string? MachineLossReason { get; set; }

    /// <summary>
    /// Thừa PO - Cuộn dưới 5kg
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ExcessPOLess5Kg { get; set; }

    /// <summary>
    /// Thừa PO - Cuộn trên 5kg
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ExcessPOOver5Kg { get; set; }

    /// <summary>
    /// Thừa PO - Hàng cắt (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ExcessPOCut { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool? BtpWarehouseConfirmed { get; set; }

    /// <summary>
    /// Tồn (Kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal RemainingInventoryKg { get; set; }
}
