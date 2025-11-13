using System.ComponentModel.DataAnnotations;

namespace Fox.Whs.Dtos;

/// <summary>
/// DTO cho việc tạo công đoạn thổi
/// </summary>
public record CreateBlowingProcessDto
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
    /// Danh sách công nhân thổi
    /// </summary>
    [StringLength(1000)]
    public string? ListOfWorkersText { get; set; }

    /// <summary>
    /// Danh sách chi tiết công đoạn thổi
    /// </summary>
    public List<CreateBlowingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc cập nhật công đoạn thổi
/// </summary>
public record UpdateBlowingProcessDto
{
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
    /// Danh sách công nhân thổi
    /// </summary>
    [StringLength(1000)]
    public string? ListOfWorkersText { get; set; }

    public int ShiftLeaderId { get; set; }

    /// <summary>
    /// Danh sách chi tiết công đoạn thổi
    /// </summary>
    public List<UpdateBlowingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc tạo chi tiết công đoạn thổi
/// </summary>
public record CreateBlowingProcessLineDto
{
    /// <summary>
    /// ID lệnh sản xuất
    /// </summary>
    public int ProductionOrderId { get; set; }

    /// <summary>
    /// Máy thổi
    /// </summary>
    [StringLength(50)]
    public string? BlowingMachine { get; set; }

    /// <summary>
    /// ID công nhân thổi
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ thổi (kg/giờ)
    /// </summary>
    [StringLength(50)]
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
    [Range(0, int.MaxValue)]
    public int StopDurationMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    [StringLength(500)]
    public string? StopReason { get; set; }

    /// <summary>
    /// Sản lượng thổi (số cuộn)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal QuantityRolls { get; set; }

    /// <summary>
    /// Sản lượng thổi (số kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Sản lượng tua/chia/tờ (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal RewindOrSplitWeight { get; set; }

    /// <summary>
    /// Sản lượng dự trữ (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
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
    /// Trạng thái
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Ngày hoàn thành thực tế
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// Nguyên nhân chậm tiến độ (QLSX ghi)
    /// </summary>
    [StringLength(500)]
    public string? DelayReason { get; set; }

    /// <summary>
    /// Đổi khổ
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal WidthChange { get; set; }

    /// <summary>
    /// Tráng lòng
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal InnerCoating { get; set; }

    /// <summary>
    /// Cắt via
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal TrimmedEdge { get; set; }

    /// <summary>
    /// Sự cố điện
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ElectricalIssue { get; set; }

    /// <summary>
    /// DC nguyên liệu (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MaterialLossKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC nguyên liệu
    /// </summary>
    [StringLength(500)]
    public string? MaterialLossReason { get; set; }

    /// <summary>
    /// DC con người (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal HumanErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC con người
    /// </summary>
    [StringLength(500)]
    public string? HumanErrorReason { get; set; }

    /// <summary>
    /// DC lỗi máy (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MachineErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC lỗi máy
    /// </summary>
    [StringLength(500)]
    public string? MachineErrorReason { get; set; }

    /// <summary>
    /// DC lỗi khác (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal OtherErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC lỗi khác
    /// </summary>
    [StringLength(500)]
    public string? OtherErrorReason { get; set; }

    /// <summary>
    /// Thừa PO
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ExcessPO { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool SemiProductWarehouseConfirmed { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [StringLength(1000)]
    public string? Note { get; set; }

    /// <summary>
    /// Tồn kho công đoạn Thổi
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal BlowingStageInventory { get; set; }
}

/// <summary>
/// DTO cho việc cập nhật chi tiết công đoạn thổi
/// </summary>
public record UpdateBlowingProcessLineDto
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
    /// Máy thổi
    /// </summary>
    [StringLength(50)]
    public string? BlowingMachine { get; set; }

    /// <summary>
    /// ID công nhân thổi
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tốc độ thổi (kg/giờ)
    /// </summary>
    [StringLength(50)]
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
    [Range(0, int.MaxValue)]
    public int StopDurationMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    [StringLength(500)]
    public string? StopReason { get; set; }

    /// <summary>
    /// Sản lượng thổi (số cuộn)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal QuantityRolls { get; set; }

    /// <summary>
    /// Sản lượng thổi (số kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Sản lượng tua/chia/tờ (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal RewindOrSplitWeight { get; set; }

    /// <summary>
    /// Sản lượng dự trữ (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ReservedWeight { get; set; }

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
    /// Nguyên nhân chậm tiến độ (QLSX ghi)
    /// </summary>
    [StringLength(500)]
    public string? DelayReason { get; set; }

    /// <summary>
    /// Đổi khổ
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal WidthChange { get; set; }

    /// <summary>
    /// Tráng lòng
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal InnerCoating { get; set; }

    /// <summary>
    /// Cắt via
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal TrimmedEdge { get; set; }

    /// <summary>
    /// Sự cố điện
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ElectricalIssue { get; set; }

    /// <summary>
    /// DC nguyên liệu (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MaterialLossKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC nguyên liệu
    /// </summary>
    [StringLength(500)]
    public string? MaterialLossReason { get; set; }

    /// <summary>
    /// DC con người (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal HumanErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC con người
    /// </summary>
    [StringLength(500)]
    public string? HumanErrorReason { get; set; }

    /// <summary>
    /// DC lỗi máy (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal MachineErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC lỗi máy
    /// </summary>
    [StringLength(500)]
    public string? MachineErrorReason { get; set; }

    /// <summary>
    /// DC lỗi khác (kg)
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal OtherErrorKg { get; set; }

    /// <summary>
    /// Ghi rõ nguyên nhân DC lỗi khác
    /// </summary>
    [StringLength(500)]
    public string? OtherErrorReason { get; set; }

    /// <summary>
    /// Thừa PO
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal ExcessPO { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool SemiProductWarehouseConfirmed { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    [StringLength(1000)]
    public string? Note { get; set; }

    /// <summary>
    /// Tồn kho công đoạn Thổi
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal BlowingStageInventory { get; set; }
}
