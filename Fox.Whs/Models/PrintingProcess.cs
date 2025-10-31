using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.SapModels;

namespace Fox.Whs.Models;

public class PrintingProcessLine
{
    [ForeignKey("BlowingProcessId"), JsonIgnore]
    public BlowingProcess? BlowingProcess { get; set; }

    /// <summary>
    /// Mã hàng
    /// </summary>
    public string ItemCode { get; set; } = null!;

    /// <summary>
    /// Lô sản xuất
    /// </summary>
    public string ProductionBatch { get; set; } = null!;

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
    /// Độ dày / 1 lá
    /// </summary>
    public decimal? Thickness { get; set; }

    /// <summary>
    /// Khổ màng BTP
    /// </summary>
    public string BtpFilmSize { get; set; }

    /// <summary>
    /// Tên hình in
    /// </summary>
    public string PrintPatternName { get; set; }

    /// <summary>
    /// Số màu in
    /// </summary>
    public int? ColorCount { get; set; }

    /// <summary>
    /// Máy in
    /// </summary>
    public string PrintingMachine { get; set; }

    /// <summary>
    /// Tên công nhân in
    /// </summary>
    public string PrinterName { get; set; }

    /// <summary>
    /// Tốc độ in
    /// </summary>
    public decimal? PrintingSpeed { get; set; }

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
    public int? MachineStopMinutes { get; set; }

    /// <summary>
    /// Nguyên nhân dừng máy
    /// </summary>
    public string StopReason { get; set; }

    // --- Sản lượng in ---
    /// <summary>
    /// Số cuộn
    /// </summary>
    public int? RollCount { get; set; }

    /// <summary>
    /// Số chiếc
    /// </summary>
    public int? PieceCount { get; set; }

    /// <summary>
    /// Số kg
    /// </summary>
    public decimal? WeightKg { get; set; }

    /// <summary>
    /// Ngày cần hàng
    /// </summary>
    public DateTime? RequiredDate { get; set; }

    /// <summary>
    /// Xác nhận hoàn thành
    /// </summary>
    public bool? CompletionConfirmed { get; set; }

    /// <summary>
    /// Chậm tiến độ - ngày hoàn thành thực tế
    /// </summary>
    public DateTime? ActualCompletionDate { get; set; }

    /// <summary>
    /// Nguyên nhân chậm tiến độ
    /// </summary>
    public string DelayReason { get; set; }

    // --- DC gia công ---
    /// <summary>
    /// DC gia công (Kg)
    /// </summary>
    public decimal? ProcessingLossKg { get; set; }

    /// <summary>
    /// DC gia công - nguyên nhân
    /// </summary>
    public string ProcessingLossReason { get; set; }

    // --- DC do công đoạn thổi ---
    /// <summary>
    /// DC do công đoạn thổi (Kg)
    /// </summary>
    public decimal? BlowingLossKg { get; set; }

    /// <summary>
    /// DC do công đoạn thổi - nguyên nhân
    /// </summary>
    public string BlowingLossReason { get; set; }

    // --- DC do công đoạn in ---
    /// <summary>
    /// Đầu cuộn OPP (Kg)
    /// </summary>
    public decimal? OppRollHeadKg { get; set; }

    /// <summary>
    /// Đầu cuộn OPP - nguyên nhân
    /// </summary>
    public string OppRollHeadReason { get; set; }

    /// <summary>
    /// Con người (Kg)
    /// </summary>
    public decimal? HumanLossKg { get; set; }

    /// <summary>
    /// Con người - nguyên nhân
    /// </summary>
    public string HumanLossReason { get; set; }

    /// <summary>
    /// Lỗi máy (Kg)
    /// </summary>
    public decimal? MachineLossKg { get; set; }

    /// <summary>
    /// Lỗi máy - nguyên nhân
    /// </summary>
    public string MachineLossReason { get; set; }

    /// <summary>
    /// Tổng DC (Kg)
    /// </summary>
    public decimal? TotalLossKg { get; set; }

    /// <summary>
    /// Thừa PO
    /// </summary>
    public bool? PoSurplus { get; set; }

    /// <summary>
    /// Xác nhận của kho BTP
    /// </summary>
    public bool? BtpWarehouseConfirmation { get; set; }

    /// <summary>
    /// Tồn kho ở công đoạn In (Kg)
    /// </summary>
    public decimal? PrintingStageInventoryKg { get; set; }
}
