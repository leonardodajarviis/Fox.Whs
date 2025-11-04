using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

/// <summary>
/// Service quản lý công đoạn cắt
/// </summary>
public class CuttingProcessService
{
    private readonly AppDbContext _dbContext;

    private readonly UserContextService _userContextService;

    public CuttingProcessService(
        AppDbContext dbContext,
        UserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn cắt
    /// </summary>
    public async Task<PaginationResponse<CuttingProcess>> GetAllAsync(QueryParam pr)
    {
        var query = _dbContext.CuttingProcesses.AsNoTracking().ApplyFiltering(pr).AsQueryable();

        var totalCount = await query.CountAsync();

        var result = await query
            .Include(cp => cp.ShiftLeader)
            .ApplyOrderingAndPaging(pr)
            .OrderByDescending(cp => cp.ProductionDate)
            .ToListAsync();

        return new PaginationResponse<CuttingProcess>
        {
            Results = result,
            TotalCount = totalCount,
            PageSize = pr.PageSize,
            Page = pr.Page,
        };
    }

    public async Task<CuttingProcess> GetByIdAsync(int id)
    {
        var cuttingProcess = await _dbContext.CuttingProcesses
            .Include(cp => cp.ShiftLeader)
            .Include(cp => cp.Lines)
                .ThenInclude(line => line.Worker)
            .Include(cp => cp.Lines)
                .ThenInclude(line => line.BusinessPartner)
            .FirstOrDefaultAsync(cp => cp.Id == id);

        if (cuttingProcess == null)
        {
            throw new NotFoundException($"Không tìm với ID: {id}");
        }

        return cuttingProcess;
    }

    /// <summary>
    /// Tạo công đoạn cắt mới
    /// </summary>
    public async Task<CuttingProcess> CreateAsync(CreateCuttingProcessDto dto)
    {
        var currentEmployeeId = _userContextService.GetCurrentEmployeeId() ?? throw new UnauthorizedException("Không xác định được nhân viên hiện tại");
        var currentUserId = _userContextService.GetCurrentUserId() ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");

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
            .ThenInclude(po => po!.ProductTypeInfo)
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        var lines = new List<CuttingProcessLine>();
        foreach (var lineDto in dto.Lines)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");
            var item = productionOrder.ItemDetail ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");

            var line = MapCreateToCuttingProcessLine(
                lineDto,
                productionOrder.ItemCode,
                productionOrder?.CardCode ?? string.Empty,
                productionOrder?.ProductionBatch,
                productionOrder?.DateOfNeedCutting,
                item.ProductType,
                item.ProductTypeName,
                item.Thickness,
                item.SemiProductWidth,
                item.Size,
                item.ColorCount);

            lines.Add(line);
        }

        var cuttingProcess = new CuttingProcess
        {
            ShiftLeaderId = currentEmployeeId,
            ProductionDate = dto.ProductionDate,
            IsDraft = dto.IsDraft,
            ProductionShift = dto.ProductionShift,
            CreatorId = currentUserId,
            Lines = lines
        };

        // Tính toán tổng
        CalculateTotals(cuttingProcess);


        _dbContext.CuttingProcesses.Add(cuttingProcess);
        await _dbContext.SaveChangesAsync();


        return await GetByIdAsync(cuttingProcess.Id);
    }

    /// <summary>
    /// Cập nhật công đoạn cắt
    /// </summary>
    public async Task<CuttingProcess> UpdateAsync(int id, UpdateCuttingProcessDto dto)
    {

        var currentUserId = _userContextService.GetCurrentUserId() ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");

        var cuttingProcess = await _dbContext.CuttingProcesses
            .Include(cp => cp.Lines)
            .FirstOrDefaultAsync(cp => cp.Id == id);

        if (cuttingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn cắt với ID: {id}");
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
            .ThenInclude(po => po!.ProductTypeInfo)
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        // Cập nhật thông tin cơ bản
        cuttingProcess.ProductionDate = dto.ProductionDate;
        cuttingProcess.ProductionShift = dto.ProductionShift;
        cuttingProcess.IsDraft = dto.IsDraft;
        cuttingProcess.ModifierId = currentUserId;
        cuttingProcess.ModifiedAt = DateTime.Now;

        // Cập nhật lines
        UpdateLines(cuttingProcess, dto.Lines, existingProductionOrders);

        // Tính toán lại tổng
        CalculateTotals(cuttingProcess);

        await _dbContext.SaveChangesAsync();


        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Xóa công đoạn cắt
    /// </summary>
    public async Task DeleteAsync(int id)
    {

        var cuttingProcess = await _dbContext.CuttingProcesses
            .Include(cp => cp.Lines)
            .FirstOrDefaultAsync(cp => cp.Id == id);

        if (cuttingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn cắt với ID: {id}");
        }

        _dbContext.CuttingProcesses.Remove(cuttingProcess);
        await _dbContext.SaveChangesAsync();

    }

    #region Private Methods

    private static CuttingProcessLine MapCreateToCuttingProcessLine(
        CreateCuttingProcessLineDto dto,
        string itemCode,
        string? cardCode,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? productTypeName,
        string? thickness,
        string? semiProductWidth,
        string? size,
        string? colorCount
    )
    {
        var line = new CuttingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            CardCode = cardCode,
            ProductionBatch = productionBatch,
            ProductType = productType,
            ProductTypeName = productTypeName,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            Size = size,
            ColorCount = colorCount,
            CuttingMachine = dto.CuttingMachine,
            WorkerId = dto.WorkerId,
            CuttingSpeed = dto.CuttingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MachineStopMinutes = dto.MachineStopMinutes,
            StopReason = dto.StopReason,
            PieceCount = dto.PieceCount,
            QuantityKg = dto.QuantityKg,
            BagCount = dto.BagCount,
            FoldedCount = dto.FoldedCount,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            ProcessingLossKg = dto.ProcessingLossKg,
            ProcessingLossReason = dto.ProcessingLossReason,
            BlowingLossKg = dto.BlowingLossKg,
            BlowingLossReason = dto.BlowingLossReason,
            PrintingLossKg = dto.PrintingLossKg,
            PrintingLossReason = dto.PrintingLossReason,
            PrintingMachine = dto.PrintingMachine,
            TransferKg = dto.TransferKg,
            HumanLossKg = dto.HumanLossKg,
            HumanLossReason = dto.HumanLossReason,
            MachineLossKg = dto.MachineLossKg,
            MachineLossReason = dto.MachineLossReason,
            ExcessPOLess5Kg = dto.ExcessPOLess5Kg,
            ExcessPOOver5Kg = dto.ExcessPOOver5Kg,
            ExcessPOCut = dto.ExcessPOCut,
            BtpWarehouseConfirmed = dto.BtpWarehouseConfirmed,
            RemainingInventoryKg = dto.RemainingInventoryKg
        };

        // Tính toán tổng DC cho line
        line.TotalLossKg = line.ProcessingLossKg + line.BlowingLossKg +
                          line.PrintingLossKg + line.HumanLossKg +
                          line.MachineLossKg;

        return line;
    }

    private static CuttingProcessLine MapUpdateToCuttingProcessLine(
        UpdateCuttingProcessLineDto dto,
        string itemCode,
        string? cardCode,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? productTypeName,
        string? thickness,
        string? semiProductWidth,
        string? size,
        string? colorCount,
        int? existingId = null
    )
    {
        var line = new CuttingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            CardCode = cardCode,
            ProductionBatch = productionBatch,
            ProductType = productType,
            ProductTypeName = productTypeName,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            Size = size,
            ColorCount = colorCount,
            CuttingMachine = dto.CuttingMachine,
            WorkerId = dto.WorkerId,
            CuttingSpeed = dto.CuttingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MachineStopMinutes = dto.MachineStopMinutes,
            StopReason = dto.StopReason,
            PieceCount = dto.PieceCount,
            QuantityKg = dto.QuantityKg,
            BagCount = dto.BagCount,
            FoldedCount = dto.FoldedCount,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            ProcessingLossKg = dto.ProcessingLossKg,
            ProcessingLossReason = dto.ProcessingLossReason,
            BlowingLossKg = dto.BlowingLossKg,
            BlowingLossReason = dto.BlowingLossReason,
            PrintingLossKg = dto.PrintingLossKg,
            PrintingLossReason = dto.PrintingLossReason,
            PrintingMachine = dto.PrintingMachine,
            TransferKg = dto.TransferKg,
            HumanLossKg = dto.HumanLossKg,
            HumanLossReason = dto.HumanLossReason,
            MachineLossKg = dto.MachineLossKg,
            MachineLossReason = dto.MachineLossReason,
            ExcessPOLess5Kg = dto.ExcessPOLess5Kg,
            ExcessPOOver5Kg = dto.ExcessPOOver5Kg,
            ExcessPOCut = dto.ExcessPOCut,
            BtpWarehouseConfirmed = dto.BtpWarehouseConfirmed,
            RemainingInventoryKg = dto.RemainingInventoryKg
        };

        if (existingId.HasValue)
        {
            line.Id = existingId.Value;
        }

        // Tính toán tổng DC cho line
        line.TotalLossKg = line.ProcessingLossKg + line.BlowingLossKg +
                          line.PrintingLossKg + line.HumanLossKg +
                          line.MachineLossKg;

        return line;
    }

    private void UpdateLines(
        CuttingProcess cuttingProcess,
        List<UpdateCuttingProcessLineDto> lineDtos,
        Dictionary<int, ProductionOrder> existingProductionOrders
    )
    {
        // Xóa các line không còn tồn tại trong DTO
        var dtoLineIds = lineDtos
            .Where(dto => dto.Id.HasValue)
            .Select(dto => dto.Id!.Value)
            .ToHashSet();

        var linesToRemove = cuttingProcess.Lines
            .Where(line => !dtoLineIds.Contains(line.Id))
            .ToList();

        foreach (var line in linesToRemove)
        {
            cuttingProcess.Lines.Remove(line);
            _dbContext.CuttingProcessLines.Remove(line);
        }

        // Cập nhật hoặc thêm mới các line
        foreach (var lineDto in lineDtos)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");
            var item = productionOrder.ItemDetail ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");

            if (lineDto.Id.HasValue)
            {
                // Cập nhật line hiện có
                var existingLine = cuttingProcess.Lines.FirstOrDefault(l => l.Id == lineDto.Id.Value);
                if (existingLine != null)
                {
                    var updatedLine = MapUpdateToCuttingProcessLine(lineDto, productionOrder.ItemCode, productionOrder?.CardCode, productionOrder?.ProductionBatch, productionOrder?.DateOfNeedCutting, item.ProductType,item.ProductTypeName, item.Thickness, item.SemiProductWidth, item.Size, item.ColorCount, lineDto.Id);
                    updatedLine.CuttingProcessId = existingLine.CuttingProcessId; // Giữ nguyên khóa ngoại
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapUpdateToCuttingProcessLine(lineDto, productionOrder.ItemCode, productionOrder?.CardCode, productionOrder?.ProductionBatch, productionOrder?.DateOfNeedCutting, item.ProductType, item.ProductTypeName, item.Thickness, item.SemiProductWidth, item.Size, item.ColorCount);
                cuttingProcess.Lines.Add(newLine);
            }
        }
    }

    private static void CalculateTotals(CuttingProcess cuttingProcess)
    {
        cuttingProcess.TotalCuttingOutput = cuttingProcess.Lines.Sum(l => l.QuantityKg);
        cuttingProcess.TotalFoldedCount = cuttingProcess.Lines.Sum(l => l.FoldedCount);
        cuttingProcess.TotalProcessingMold = cuttingProcess.Lines.Sum(l => l.ProcessingLossKg);
    }

    #endregion
}
