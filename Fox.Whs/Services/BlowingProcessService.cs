using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

/// <summary>
/// Service quản lý công đoạn thổi
/// </summary>
public class BlowingProcessService
{
    private readonly AppDbContext _dbContext;
    private readonly UserContextService _userContextService;

    public BlowingProcessService(
        AppDbContext dbContext,
        UserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn thổi
    /// </summary>
    public async Task<PaginationResponse<BlowingProcess>> GetAllAsync(QueryParam pr)
    {
        var query = _dbContext.BlowingProcesses.AsNoTracking().ApplyFiltering(pr).AsQueryable();


        var totalCount = await query.CountAsync();

        var result = await query
            .Include(pp => pp.Creator)
            .Include(pp => pp.Modifier)
            .Include(bp => bp.ShiftLeader)
            .ApplyOrderingAndPaging(pr)
            .OrderByDescending(bp => bp.ProductionDate)
            .ToListAsync();

        return new PaginationResponse<BlowingProcess>
        {
            Results = result,
            TotalCount = totalCount,
            PageSize = pr.PageSize,
            Page = pr.Page,
        };
    }

    public async Task<BlowingProcess> GetByIdAsync(int id)
    {
        var blowingProcess = await _dbContext.BlowingProcesses.AsNoTracking()
            .Include(pp => pp.Creator)
            .Include(pp => pp.Modifier)
            .Include(bp => bp.ShiftLeader)
            .Include(bp => bp.Lines)
                .ThenInclude(line => line.Worker)
            .FirstOrDefaultAsync(bp => bp.Id == id);

        if (blowingProcess == null)
        {
            throw new NotFoundException($"Không tìm với ID: {id}");
        }

        return blowingProcess;
    }

    /// <summary>
    /// Tạo công đoạn thổi mới
    /// </summary>
    public async Task<BlowingProcess> CreateAsync(CreateBlowingProcessDto dto)
    {
        var shiftLeaderId = dto.ShiftLeaderId ??
            _userContextService.GetCurrentEmployeeId() ?? throw new UnauthorizedException("Không xác định được nhân viên hiện tại");

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

        var lines = new List<BlowingProcessLine>();
        foreach (var lineDto in dto.Lines)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");
            var item = productionOrder.ItemDetail ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");

            var line = MapCreateToBlowingProcessLine(
                lineDto,
                productionOrder.ItemCode,
                productionOrder?.CardCode ?? string.Empty,
                productionOrder?.ProductionBatch,
                productionOrder?.DateOfNeedBlowing,
                item.ProductType,
                item.ProductTypeName,
                item.Thickness,
                item.SemiProductWidth);

            lines.Add(line);
        }

        var blowingProcess = new BlowingProcess
        {
            ShiftLeaderId = shiftLeaderId,
            CreatorId = currentUserId,
            ProductionDate = dto.ProductionDate,
            IsDraft = dto.IsDraft,
            ProductionShift = dto.ProductionShift,
            ListOfWorkersText = dto.ListOfWorkersText,
            Lines = lines
        };

        // Tính toán tổng
        CalculateTotals(blowingProcess);

        _dbContext.BlowingProcesses.Add(blowingProcess);
        await _dbContext.SaveChangesAsync();


        return await GetByIdAsync(blowingProcess.Id);
    }

    /// <summary>
    /// Cập nhật công đoạn thổi
    /// </summary>
    public async Task<BlowingProcess> UpdateAsync(int id, UpdateBlowingProcessDto dto)
    {
        var currentUserId = _userContextService.GetCurrentUserId() ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");

        var blowingProcess = await _dbContext.BlowingProcesses
            .Include(bp => bp.Lines)
            .FirstOrDefaultAsync(bp => bp.Id == id);

        if (blowingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn thổi với ID: {id}");
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
        blowingProcess.ProductionDate = dto.ProductionDate;
        blowingProcess.ProductionShift = dto.ProductionShift;
        blowingProcess.IsDraft = dto.IsDraft;
        blowingProcess.ModifierId = currentUserId;
        blowingProcess.ModifiedAt = DateTime.Now;
        blowingProcess.ListOfWorkersText = dto.ListOfWorkersText;
        blowingProcess.ShiftLeaderId = dto.ShiftLeaderId;

        // Cập nhật lines
        UpdateLines(blowingProcess, dto.Lines, existingProductionOrders);

        // Tính toán lại tổng
        CalculateTotals(blowingProcess);

        if (!blowingProcess.IsDraft)
        {
            var productOrderCompletedIds = blowingProcess.Lines
                .Where(l => l.Status == 1)
                .Select(l => l.ProductionOrderId)
                .Distinct()
                .ToArray() ?? [];

            if (productOrderCompletedIds.Length > 0)
            {
                await _dbContext.UpdateStatusProductionOrderSapAsync("U_THOISTATUS", productOrderCompletedIds);
            }

            if (blowingProcess.Lines.All(l => l.Status == 1))
            {
                blowingProcess.Status = 1; // Hoàn thành
            }
            else if (blowingProcess.Lines.Any(l => l.Status == 1))
            {
                blowingProcess.Status = 2; // Đang tiến hành
            }
            else if (blowingProcess.Lines.All(l => l.Status == 0))
            {
                blowingProcess.Status = 0;
            }
        }

        await _dbContext.SaveChangesAsync();


        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Xóa công đoạn thổi
    /// </summary>
    public async Task DeleteAsync(int id)
    {

        var blowingProcess = await _dbContext.BlowingProcesses
            .Include(bp => bp.Lines)
            .FirstOrDefaultAsync(bp => bp.Id == id);

        if (blowingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn thổi với ID: {id}");
        }

        _dbContext.BlowingProcesses.Remove(blowingProcess);
        await _dbContext.SaveChangesAsync();

    }

    #region Private Methods

    private static BlowingProcessLine MapCreateToBlowingProcessLine(
        CreateBlowingProcessLineDto dto,
        string itemCode,
        string? cardCode,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? productTypeName,
        string? thickness,
        string? semiProductWidth
    )
    {
        var line = new BlowingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            CardCode = cardCode,
            ProductType = productType,
            ProductTypeName = productTypeName,
            ProductionBatch = productionBatch,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            BlowingMachine = dto.BlowingMachine,
            WorkerId = dto.WorkerId,
            BlowingSpeed = dto.BlowingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            StopDurationMinutes = dto.StopDurationMinutes,
            StopReason = dto.StopReason,
            QuantityRolls = dto.QuantityRolls,
            QuantityKg = dto.QuantityKg,
            RewindOrSplitWeight = dto.RewindOrSplitWeight,
            ReservedWeight = dto.ReservedWeight,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            WidthChange = dto.WidthChange,
            InnerCoating = dto.InnerCoating,
            TrimmedEdge = dto.TrimmedEdge,
            ElectricalIssue = dto.ElectricalIssue,
            MaterialLossKg = dto.MaterialLossKg,
            MaterialLossReason = dto.MaterialLossReason,
            HumanErrorKg = dto.HumanErrorKg,
            HumanErrorReason = dto.HumanErrorReason,
            MachineErrorKg = dto.MachineErrorKg,
            MachineErrorReason = dto.MachineErrorReason,
            OtherErrorKg = dto.OtherErrorKg,
            OtherErrorReason = dto.OtherErrorReason,
            ExcessPO = dto.ExcessPO,
            SemiProductWarehouseConfirmed = dto.SemiProductWarehouseConfirmed,
            Note = dto.Note,
            BlowingStageInventory = dto.BlowingStageInventory
        };

        // Tính toán tổng DC cho line
        line.TotalLoss = line.WidthChange + line.InnerCoating + line.TrimmedEdge +
                        line.ElectricalIssue + line.MaterialLossKg +
                        line.HumanErrorKg + line.MachineErrorKg + line.OtherErrorKg;

        return line;
    }

    private static BlowingProcessLine MapUpdateToBlowingProcessLine(
        UpdateBlowingProcessLineDto dto,
        string itemCode,
        string? cardCode,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? productTypeName,
        string? thickness,
        string? semiProductWidth,
        int? existingId = null
    )
    {
        var line = new BlowingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            CardCode = cardCode,
            ProductionBatch = productionBatch,
            ProductType = productType,
            ProductTypeName = productTypeName,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            BlowingMachine = dto.BlowingMachine,
            WorkerId = dto.WorkerId,
            BlowingSpeed = dto.BlowingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            StopDurationMinutes = dto.StopDurationMinutes,
            StopReason = dto.StopReason,
            QuantityRolls = dto.QuantityRolls,
            QuantityKg = dto.QuantityKg,
            RewindOrSplitWeight = dto.RewindOrSplitWeight,
            ReservedWeight = dto.ReservedWeight,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            WidthChange = dto.WidthChange,
            InnerCoating = dto.InnerCoating,
            TrimmedEdge = dto.TrimmedEdge,
            ElectricalIssue = dto.ElectricalIssue,
            MaterialLossKg = dto.MaterialLossKg,
            MaterialLossReason = dto.MaterialLossReason,
            HumanErrorKg = dto.HumanErrorKg,
            HumanErrorReason = dto.HumanErrorReason,
            MachineErrorKg = dto.MachineErrorKg,
            MachineErrorReason = dto.MachineErrorReason,
            OtherErrorKg = dto.OtherErrorKg,
            OtherErrorReason = dto.OtherErrorReason,
            ExcessPO = dto.ExcessPO,
            SemiProductWarehouseConfirmed = dto.SemiProductWarehouseConfirmed,
            Note = dto.Note,
            BlowingStageInventory = dto.BlowingStageInventory
        };

        if (existingId.HasValue)
        {
            line.Id = existingId.Value;
        }

        // Tính toán tổng DC cho line
        line.TotalLoss = line.WidthChange + line.InnerCoating + line.TrimmedEdge +
                        line.ElectricalIssue + line.MaterialLossKg +
                        line.HumanErrorKg + line.MachineErrorKg + line.OtherErrorKg;

        return line;
    }

    private void UpdateLines(
        BlowingProcess blowingProcess,
        List<UpdateBlowingProcessLineDto> lineDtos,
        Dictionary<int, ProductionOrder> existingProductionOrders
    )
    {
        // Xóa các line không còn tồn tại trong DTO
        var dtoLineIds = lineDtos
            .Where(dto => dto.Id.HasValue)
            .Select(dto => dto.Id!.Value)
            .ToHashSet();

        var linesToRemove = blowingProcess.Lines
            .Where(line => !dtoLineIds.Contains(line.Id))
            .ToList();

        foreach (var line in linesToRemove)
        {
            blowingProcess.Lines.Remove(line);
            _dbContext.BlowingProcessLines.Remove(line);
        }

        // Cập nhật hoặc thêm mới các line
        foreach (var lineDto in lineDtos)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");
            var item = productionOrder.ItemDetail ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");
            if (lineDto.Id.HasValue)
            {
                // Cập nhật line hiện có
                var existingLine = blowingProcess.Lines.FirstOrDefault(l => l.Id == lineDto.Id.Value);
                if (existingLine != null)
                {
                    var updatedLine = MapUpdateToBlowingProcessLine(lineDto, productionOrder.ItemCode, productionOrder?.CardCode, productionOrder?.ProductionBatch, productionOrder?.DateOfNeedBlowing, item.ProductType, item.ProductTypeName, item.Thickness, item.SemiProductWidth, lineDto.Id);
                    updatedLine.BlowingProcessId = existingLine.BlowingProcessId; // Giữ nguyên khóa ngoại
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapUpdateToBlowingProcessLine(lineDto, productionOrder.ItemCode, productionOrder?.CardCode, productionOrder?.ProductionBatch, productionOrder?.DateOfNeedBlowing, item.ProductType, item.ProductTypeName, item.Thickness, item.SemiProductWidth);
                blowingProcess.Lines.Add(newLine);
            }
        }
    }

    private static void CalculateTotals(BlowingProcess blowingProcess)
    {
        blowingProcess.TotalBlowingOutput = blowingProcess.Lines.Sum(l => l.QuantityKg);
        blowingProcess.TotalRewindingOutput = blowingProcess.Lines.Sum(l => l.RewindOrSplitWeight);
        blowingProcess.TotalReservedOutput = blowingProcess.Lines.Sum(l => l.ReservedWeight);
        blowingProcess.TotalBlowingLoss = blowingProcess.Lines.Sum(l => l.TotalLoss);
    }

    #endregion
}
