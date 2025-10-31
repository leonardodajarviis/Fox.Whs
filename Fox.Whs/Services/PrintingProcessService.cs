using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

/// <summary>
/// Service quản lý công đoạn in
/// </summary>
public class PrintingProcessService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<PrintingProcessService> _logger;

    public PrintingProcessService(
        AppDbContext dbContext,
        ILogger<PrintingProcessService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn in
    /// </summary>
    public async Task<PaginationResponse<PrintingProcess>> GetAllAsync(QueryParam pr)
    {
        var query = _dbContext.PrintingProcesses.AsNoTracking().ApplyFiltering(pr).AsQueryable();

        var totalCount = await query.CountAsync();

        var result = await query
            .ApplyOrderingAndPaging(pr)
            .OrderByDescending(pp => pp.ProductionDate)
            .ToListAsync();

        return new PaginationResponse<PrintingProcess>
        {
            Results = result,
            TotalCount = totalCount,
            PageSize = pr.PageSize,
            Page = pr.Page,
        };
    }

    public async Task<PrintingProcess> GetByIdAsync(int id)
    {
        var printingProcess = await _dbContext.PrintingProcesses
            .Include(pp => pp.ShiftLeader)
            .Include(pp => pp.Lines)
                .ThenInclude(line => line.Worker)
            .Include(pp => pp.Lines)
                .ThenInclude(line => line.BusinessPartner)
            .FirstOrDefaultAsync(pp => pp.Id == id);

        if (printingProcess == null)
        {
            throw new NotFoundException($"Không tìm với ID: {id}");
        }

        return printingProcess;
    }

    /// <summary>
    /// Tạo công đoạn in mới
    /// </summary>
    public async Task<PrintingProcess> CreateAsync(CreatePrintingProcessDto dto)
    {
        _logger.LogInformation("Tạo công đoạn in mới cho trưởng ca {LeaderId}", dto.ShiftLeaderId);

        // Kiểm tra trưởng ca tồn tại
        var shiftLeaderExists = await _dbContext.Employees
            .AnyAsync(e => e.Id == dto.ShiftLeaderId);

        if (!shiftLeaderExists)
        {
            throw new NotFoundException($"Không tìm thấy trưởng ca với ID: {dto.ShiftLeaderId}");
        }

        var workerIds = dto.Lines
            .Where(l => l.WorkerId.HasValue)
            .Select(l => l.WorkerId!.Value)
            .Distinct()
            .ToList();

        var existingWorkers = await _dbContext.Employees
            .Where(e => workerIds.Contains(e.Id))
            .ToDictionaryAsync(w => w.Id);

        var productionOrderIds = dto.Lines
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToList();

        var existingProductionOrders = await _dbContext.ProductionOrders
            .Include(po => po.ItemDetail)
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        var lines = new List<PrintingProcessLine>();
        foreach (var lineDto in dto.Lines)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");
            var item = productionOrder.ItemDetail ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");

            var line = MapCreateToPrintingProcessLine(
                lineDto,
                productionOrder.ItemCode,
                productionOrder?.CardCode ?? string.Empty,
                productionOrder?.ProductionBatch,
                productionOrder?.DateOfNeedPrinting,
                item.ProductType,
                item.Thickness,
                item.SemiProductWidth);

            lines.Add(line);
        }

        var printingProcess = new PrintingProcess
        {
            ShiftLeaderId = dto.ShiftLeaderId,
            ProductionDate = dto.ProductionDate,
            IsDraft = dto.IsDraft,
            ProductionShift = dto.ProductionShift,
            Lines = lines
        };

        // Tính toán tổng
        CalculateTotals(printingProcess);

        _dbContext.PrintingProcesses.Add(printingProcess);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Đã tạo công đoạn in với ID: {Id}", printingProcess.Id);

        return await GetByIdAsync(printingProcess.Id);
    }

    /// <summary>
    /// Cập nhật công đoạn in
    /// </summary>
    public async Task<PrintingProcess> UpdateAsync(int id, UpdatePrintingProcessDto dto)
    {
        _logger.LogInformation("Cập nhật công đoạn in với ID: {Id}", id);

        var printingProcess = await _dbContext.PrintingProcesses
            .Include(pp => pp.Lines)
            .FirstOrDefaultAsync(pp => pp.Id == id);

        if (printingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn in với ID: {id}");
        }

        // Kiểm tra trưởng ca tồn tại
        var shiftLeaderExists = await _dbContext.Employees
            .AnyAsync(e => e.Id == dto.ShiftLeaderId);

        if (!shiftLeaderExists)
        {
            throw new NotFoundException($"Không tìm thấy trưởng ca với ID: {dto.ShiftLeaderId}");
        }

        var workerIds = dto.Lines
            .Where(l => l.WorkerId.HasValue)
            .Select(l => l.WorkerId!.Value)
            .Distinct()
            .ToList();

        var existingWorkers = await _dbContext.Employees
            .Where(e => workerIds.Contains(e.Id))
            .ToDictionaryAsync(w => w.Id);

        var productionOrderIds = dto.Lines
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToList();

        var existingProductionOrders = await _dbContext.ProductionOrders
            .Include(po => po.ItemDetail)
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        // Cập nhật thông tin cơ bản
        printingProcess.ShiftLeaderId = dto.ShiftLeaderId;
        printingProcess.ProductionDate = dto.ProductionDate;
        printingProcess.ProductionShift = dto.ProductionShift;

        // Cập nhật lines
        UpdateLines(printingProcess, dto.Lines, existingProductionOrders);

        // Tính toán lại tổng
        CalculateTotals(printingProcess);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Đã cập nhật công đoạn in với ID: {Id}", id);

        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Xóa công đoạn in
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Xóa công đoạn in với ID: {Id}", id);

        var printingProcess = await _dbContext.PrintingProcesses
            .Include(pp => pp.Lines)
            .FirstOrDefaultAsync(pp => pp.Id == id);

        if (printingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn in với ID: {id}");
        }

        _dbContext.PrintingProcesses.Remove(printingProcess);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Đã xóa công đoạn in với ID: {Id}", id);
    }

    #region Private Methods

    private static PrintingProcessLine MapCreateToPrintingProcessLine(
        CreatePrintingProcessLineDto dto,
        string itemCode,
        string? cardCode,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? thickness,
        string? semiProductWidth
    )
    {
        var line = new PrintingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            CardCode = cardCode,
            ProductionBatch = productionBatch,
            ProductType = productType,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            PrintPatternName = dto.PrintPatternName,
            ColorCount = dto.ColorCount,
            PrintingMachine = dto.PrintingMachine,
            WorkerId = dto.WorkerId,
            PrintingSpeed = dto.PrintingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MachineStopMinutes = dto.MachineStopMinutes,
            StopReason = dto.StopReason,
            RollCount = dto.RollCount,
            PieceCount = dto.PieceCount,
            WeightKg = dto.WeightKg,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            ProcessingLossKg = dto.ProcessingLossKg,
            ProcessingLossReason = dto.ProcessingLossReason,
            BlowingLossKg = dto.BlowingLossKg,
            BlowingLossReason = dto.BlowingLossReason,
            OppRollHeadKg = dto.OppRollHeadKg,
            OppRollHeadReason = dto.OppRollHeadReason,
            HumanLossKg = dto.HumanLossKg,
            HumanLossReason = dto.HumanLossReason,
            MachineLossKg = dto.MachineLossKg,
            MachineLossReason = dto.MachineLossReason,
            PoSurplus = dto.PoSurplus,
            BtpWarehouseConfirmation = dto.BtpWarehouseConfirmation,
            PrintingStageInventoryKg = dto.PrintingStageInventoryKg
        };

        // Tính toán tổng DC cho line
        line.TotalLossKg = line.ProcessingLossKg + line.BlowingLossKg + 
                          line.OppRollHeadKg + line.HumanLossKg + 
                          line.MachineLossKg;

        return line;
    }

    private static PrintingProcessLine MapUpdateToPrintingProcessLine(
        UpdatePrintingProcessLineDto dto,
        string itemCode,
        string? cardCode,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? thickness,
        string? semiProductWidth,
        int? existingId = null
    )
    {
        var line = new PrintingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            CardCode = cardCode,
            ProductionBatch = productionBatch,
            ProductType = productType,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            PrintPatternName = dto.PrintPatternName,
            ColorCount = dto.ColorCount,
            PrintingMachine = dto.PrintingMachine,
            WorkerId = dto.WorkerId,
            PrintingSpeed = dto.PrintingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MachineStopMinutes = dto.MachineStopMinutes,
            StopReason = dto.StopReason,
            RollCount = dto.RollCount,
            PieceCount = dto.PieceCount,
            WeightKg = dto.WeightKg,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            ProcessingLossKg = dto.ProcessingLossKg,
            ProcessingLossReason = dto.ProcessingLossReason,
            BlowingLossKg = dto.BlowingLossKg,
            BlowingLossReason = dto.BlowingLossReason,
            OppRollHeadKg = dto.OppRollHeadKg,
            OppRollHeadReason = dto.OppRollHeadReason,
            HumanLossKg = dto.HumanLossKg,
            HumanLossReason = dto.HumanLossReason,
            MachineLossKg = dto.MachineLossKg,
            MachineLossReason = dto.MachineLossReason,
            PoSurplus = dto.PoSurplus,
            BtpWarehouseConfirmation = dto.BtpWarehouseConfirmation,
            PrintingStageInventoryKg = dto.PrintingStageInventoryKg
        };

        if (existingId.HasValue)
        {
            line.Id = existingId.Value;
        }

        // Tính toán tổng DC cho line
        line.TotalLossKg = line.ProcessingLossKg + line.BlowingLossKg + 
                          line.OppRollHeadKg + line.HumanLossKg + 
                          line.MachineLossKg;

        return line;
    }

    private void UpdateLines(
        PrintingProcess printingProcess,
        List<UpdatePrintingProcessLineDto> lineDtos,
        Dictionary<int, ProductionOrder> existingProductionOrders
    )
    {
        // Xóa các line không còn tồn tại trong DTO
        var dtoLineIds = lineDtos
            .Where(dto => dto.Id.HasValue)
            .Select(dto => dto.Id!.Value)
            .ToHashSet();

        var linesToRemove = printingProcess.Lines
            .Where(line => !dtoLineIds.Contains(line.Id))
            .ToList();

        foreach (var line in linesToRemove)
        {
            printingProcess.Lines.Remove(line);
            _dbContext.PrintingProcessLines.Remove(line);
        }

        // Cập nhật hoặc thêm mới các line
        foreach (var lineDto in lineDtos)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");
            var item = productionOrder.ItemDetail ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");
            
            if (lineDto.Id.HasValue)
            {
                // Cập nhật line hiện có
                var existingLine = printingProcess.Lines.FirstOrDefault(l => l.Id == lineDto.Id.Value);
                if (existingLine != null)
                {
                    var updatedLine = MapUpdateToPrintingProcessLine(lineDto, productionOrder.ItemCode, productionOrder?.CardCode, productionOrder?.ProductionBatch, productionOrder?.DateOfNeedPrinting, item.ProductType, item.Thickness, item.SemiProductWidth, lineDto.Id);
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapUpdateToPrintingProcessLine(lineDto, productionOrder.ItemCode, productionOrder?.CardCode, productionOrder?.ProductionBatch, productionOrder?.DateOfNeedPrinting, item.ProductType, item.Thickness, item.SemiProductWidth);
                printingProcess.Lines.Add(newLine);
            }
        }
    }

    private static void CalculateTotals(PrintingProcess printingProcess)
    {
        printingProcess.TotalPrintingOutput = printingProcess.Lines.Sum(l => l.WeightKg ?? 0);
        printingProcess.TotalProcessingMold = printingProcess.Lines.Sum(l => l.ProcessingLossKg);
        printingProcess.TotalBlowingStageMold = printingProcess.Lines.Sum(l => l.BlowingLossKg);
        printingProcess.TotalPrintingStageMold = printingProcess.Lines.Sum(l => l.OppRollHeadKg + l.HumanLossKg + l.MachineLossKg);
    }

    #endregion
}
