using System.ComponentModel.DataAnnotations;

namespace Fox.Whs.Dtos;

/// <summary>
/// DTO cho việc tạo công đoạn pha hạt
/// </summary>
public record CreateGrainMixingProcessDto
{
    /// <summary>
    /// Bản nháp
    /// </summary>
    public bool IsDraft { get; set; }

    /// <summary>
    /// Ngày sản xuất (ngày pha)
    /// </summary>
    [Required(ErrorMessage = "Ngày sản xuất là bắt buộc")]
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Tổng số nhân công
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Số nhân công phải lớn hơn hoặc bằng 0")]
    public int WorkerCount { get; set; }

    /// <summary>
    /// Tổng số giờ làm việc
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Số giờ làm việc phải lớn hơn hoặc bằng 0")]
    public double TotalHoursWorked { get; set; }

    /// <summary>
    /// Danh sách chi tiết công đoạn pha hạt
    /// </summary>
    public List<CreateGrainMixingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc cập nhật công đoạn pha hạt
/// </summary>
public record UpdateGrainMixingProcessDto
{
    /// <summary>
    /// Bản nháp
    /// </summary>
    public bool IsDraft { get; set; }

    /// <summary>
    /// Ngày sản xuất (ngày pha)
    /// </summary>
    [Required(ErrorMessage = "Ngày sản xuất là bắt buộc")]
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// Tổng số nhân công
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Số nhân công phải lớn hơn hoặc bằng 0")]
    public int WorkerCount { get; set; }

    /// <summary>
    /// Tổng số giờ làm việc
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Số giờ làm việc phải lớn hơn hoặc bằng 0")]
    public double TotalHoursWorked { get; set; }

    /// <summary>
    /// Danh sách chi tiết công đoạn pha hạt
    /// </summary>
    public List<UpdateGrainMixingProcessLineDto> Lines { get; set; } = [];
}

/// <summary>
/// DTO cho việc tạo chi tiết công đoạn pha hạt
/// </summary>
public record CreateGrainMixingProcessLineDto
{
    /// <summary>
    /// Lô sản xuất
    /// </summary>
    [StringLength(100)]
    public string? ProductionBatch { get; set; }

    /// <summary>
    /// Mã khách hàng
    /// </summary>
    [MaxLength(15)]
    public string? CardCode { get; set; }

    /// <summary>
    /// Số phiếu lĩnh vật tư
    /// </summary>
    [StringLength(100)]
    public string? MaterialIssueVoucherNo { get; set; }

    /// <summary>
    /// Chủng loại pha (HD, PE, Màng co, Màng chít, PP...)
    /// </summary>
    [StringLength(100)]
    public string? MixtureType { get; set; }

    /// <summary>
    /// Quy cách diễn giải
    /// </summary>
    [StringLength(500)]
    public string? Specification { get; set; }

    /// <summary>
    /// ID công nhân pha
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tên máy pha
    /// </summary>
    [StringLength(100)]
    public string? MachineName { get; set; }

    /// <summary>
    /// Thời gian bắt đầu pha
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc pha
    /// </summary>
    public DateTime? EndTime { get; set; }

    // ------------------ PP ------------------
    public decimal PpTron { get; set; }
    public decimal PpHdNhot { get; set; }
    public decimal PpLdpe { get; set; }
    public decimal PpDc { get; set; }
    public decimal PpAdditive { get; set; }
    public decimal PpColor { get; set; }
    public decimal PpOther { get; set; }

    // ------------------ HD ------------------
    public decimal HdLldpe2320 { get; set; }
    public decimal HdRecycled { get; set; }
    public decimal HdTalcol { get; set; }
    public decimal HdDc { get; set; }
    public decimal HdColor { get; set; }
    public decimal HdOther { get; set; }

    // ------------------ PE ------------------
    public decimal PeAdditive { get; set; }
    public decimal PeTalcol { get; set; }
    public decimal PeColor { get; set; }
    public decimal PeOther { get; set; }
    public decimal PeLdpe { get; set; }
    public decimal PeLldpe { get; set; }
    public decimal PeRecycled { get; set; }

    // ------------------ Màng co ------------------
    public decimal ShrinkRe707 { get; set; }
    public decimal ShrinkSlip { get; set; }
    public decimal ShrinkStatic { get; set; }
    public decimal ShrinkDc { get; set; }
    public decimal ShrinkTalcol { get; set; }
    public decimal ShrinkOther { get; set; }

    // ------------------ Màng chít ------------------
    public decimal WrapRecycledCa { get; set; }
    public decimal WrapRecycledCb { get; set; }
    public decimal WrapGlue { get; set; }
    public decimal WrapColor { get; set; }
    public decimal WrapDc { get; set; }
    public decimal WrapLdpe { get; set; }
    public decimal WrapLldpe { get; set; }
    public decimal WrapSlip { get; set; }
    public decimal WrapAdditive { get; set; }
    public decimal WrapOther { get; set; }

    // ------------------ EVA ------------------
    public decimal EvaPop3070 { get; set; }
    public decimal EvaLdpe { get; set; }
    public decimal EvaDc { get; set; }
    public decimal EvaTalcol { get; set; }
    public decimal EvaSlip { get; set; }
    public decimal EvaStaticAdditive { get; set; }
    public decimal EvaOther { get; set; }

    /// <summary>
    /// Sản lượng pha (Kg)
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Sản lượng phải lớn hơn hoặc bằng 0")]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Ngày cần hàng
    /// </summary>
    public DateTime? RequiredDate { get; set; }

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
}

/// <summary>
/// DTO cho việc cập nhật chi tiết công đoạn pha hạt
/// </summary>
public record UpdateGrainMixingProcessLineDto
{
    /// <summary>
    /// ID của line (null nếu tạo mới)
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Lô sản xuất
    /// </summary>
    [StringLength(100)]
    public string? ProductionBatch { get; set; }

    /// <summary>
    /// Mã khách hàng
    /// </summary>
    [MaxLength(15)]
    public string? CardCode { get; set; }

    /// <summary>
    /// Số phiếu lĩnh vật tư
    /// </summary>
    [StringLength(100)]
    public string? MaterialIssueVoucherNo { get; set; }

    /// <summary>
    /// Chủng loại pha (HD, PE, Màng co, Màng chít, PP...)
    /// </summary>
    [StringLength(100)]
    public string? MixtureType { get; set; }

    /// <summary>
    /// Quy cách diễn giải
    /// </summary>
    [StringLength(500)]
    public string? Specification { get; set; }

    /// <summary>
    /// ID công nhân pha
    /// </summary>
    public int? WorkerId { get; set; }

    /// <summary>
    /// Tên máy pha
    /// </summary>
    [StringLength(100)]
    public string? MachineName { get; set; }

    /// <summary>
    /// Thời gian bắt đầu pha
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc pha
    /// </summary>
    public DateTime? EndTime { get; set; }

    // ------------------ PP ------------------
    public decimal PpTron { get; set; }
    public decimal PpHdNhot { get; set; }
    public decimal PpLdpe { get; set; }
    public decimal PpDc { get; set; }
    public decimal PpAdditive { get; set; }
    public decimal PpColor { get; set; }
    public decimal PpOther { get; set; }

    // ------------------ HD ------------------
    public decimal HdLldpe2320 { get; set; }
    public decimal HdRecycled { get; set; }
    public decimal HdTalcol { get; set; }
    public decimal HdDc { get; set; }
    public decimal HdColor { get; set; }
    public decimal HdOther { get; set; }

    // ------------------ PE ------------------
    public decimal PeAdditive { get; set; }
    public decimal PeTalcol { get; set; }
    public decimal PeColor { get; set; }
    public decimal PeOther { get; set; }
    public decimal PeLdpe { get; set; }
    public decimal PeLldpe { get; set; }
    public decimal PeRecycled { get; set; }

    // ------------------ Màng co ------------------
    public decimal ShrinkRe707 { get; set; }
    public decimal ShrinkSlip { get; set; }
    public decimal ShrinkStatic { get; set; }
    public decimal ShrinkDc { get; set; }
    public decimal ShrinkTalcol { get; set; }
    public decimal ShrinkOther { get; set; }

    // ------------------ Màng chít ------------------
    public decimal WrapRecycledCa { get; set; }
    public decimal WrapRecycledCb { get; set; }
    public decimal WrapGlue { get; set; }
    public decimal WrapColor { get; set; }
    public decimal WrapDc { get; set; }
    public decimal WrapLdpe { get; set; }
    public decimal WrapLldpe { get; set; }
    public decimal WrapSlip { get; set; }
    public decimal WrapAdditive { get; set; }
    public decimal WrapOther { get; set; }

    // ------------------ EVA ------------------
    public decimal EvaPop3070 { get; set; }
    public decimal EvaLdpe { get; set; }
    public decimal EvaDc { get; set; }
    public decimal EvaTalcol { get; set; }
    public decimal EvaSlip { get; set; }
    public decimal EvaStaticAdditive { get; set; }
    public decimal EvaOther { get; set; }

    /// <summary>
    /// Sản lượng pha (Kg)
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Sản lượng phải lớn hơn hoặc bằng 0")]
    public decimal QuantityKg { get; set; }

    /// <summary>
    /// Ngày cần hàng
    /// </summary>
    public DateTime? RequiredDate { get; set; }

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
}
