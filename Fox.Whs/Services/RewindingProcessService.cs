using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

/// <summary>
/// Service quản lý công đoạn tua
/// </summary>
public class RewindingProcessService
{
    private readonly AppDbContext _dbContext;
    private readonly UserContextService _userContextService;

    public RewindingProcessService(
        AppDbContext dbContext,
        UserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn tua
    /// </summary>
    public async Task<PaginationResponse<RewindingProcess>> GetAllAsync(QueryParam pr)
    {
        var query = _dbContext.RewindingProcesses.AsNoTracking().ApplyFiltering(pr).AsQueryable();

        var totalCount = await query.CountAsync();

        if (pr.Include == "lines")
        {
            query = query.Include(bp => bp.Lines).ThenInclude(x => x.Worker).Include(x => x.Lines).ThenInclude(x => x.BusinessPartner);
        }

        var result = await query
            .Include(rp => rp.Creator)
            .Include(rp => rp.Modifier)
            .Include(rp => rp.ShiftLeader)
            .OrderByDescending(sp => sp.Id)
            .ApplyOrderingAndPaging(pr)
            .ToListAsync();

        return new PaginationResponse<RewindingProcess>
        {
            Results = result,
            TotalCount = totalCount,
            PageSize = pr.PageSize,
            Page = pr.Page,
        };
    }

    public async Task<RewindingProcess> GetByIdAsync(int id)
    {
        var rewindingProcess = await _dbContext.RewindingProcesses
            .Include(rp => rp.Creator)
            .Include(rp => rp.Modifier)
            .Include(rp => rp.ShiftLeader)
            .Include(rp => rp.Lines)
            .ThenInclude(line => line.Worker)
            .Include(rp => rp.Lines)
            .ThenInclude(line => line.BusinessPartner)
            .FirstOrDefaultAsync(rp => rp.Id == id);

        if (rewindingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn tua với ID: {id}");
        }

        return rewindingProcess;
    }

    /// <summary>
    /// Tạo công đoạn tua mới
    /// </summary>
    public async Task<RewindingProcess> CreateAsync(CreateRewindingProcessDto dto)
    {
        var shiftLeaderId = dto.ShiftLeaderId ?? _userContextService.GetCurrentEmployeeId()
            ?? throw new UnauthorizedException("Không xác định được nhân viên hiện tại");

        var currentUserId = _userContextService.GetCurrentUserId()
            ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");

        var productionOrderIds = dto.Lines
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToList();

        var existingProductionOrders = await _dbContext.ProductionOrders
            .Include(po => po.ItemDetail)
            .ThenInclude(po => po!.ProductTypeInfo)
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        var lines = new List<RewindingProcessLine>();
        foreach (var lineDto in dto.Lines)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId)
                ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");

            var item = productionOrder.ItemDetail
                ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");

            var line = MapCreateToRewindingProcessLine(
                lineDto,
                productionOrder.ItemCode,
                productionOrder.ProdName,
                productionOrder?.CardCode ?? string.Empty,
                productionOrder?.ProductionBatch,
                productionOrder?.DateOfNeedRewinding, // RequiredDate sẽ lấy từ DTO hoặc để null
                item.ProductType,
                item.ProductTypeName,
                item.Thickness,
                item.SemiProductWidth);

            lines.Add(line);
        }

        var rewindingProcess = new RewindingProcess
        {
            ShiftLeaderId = shiftLeaderId,
            CreatorId = currentUserId,
            ProductionDate = dto.ProductionDate,
            IsDraft = dto.IsDraft,
            ProductionShift = dto.ProductionShift,
            Notes = dto.Notes,
            Lines = lines
        };

        // Tính toán tổng
        CalculateTotals(rewindingProcess);

        _dbContext.RewindingProcesses.Add(rewindingProcess);
        await _dbContext.SaveChangesAsync();

        return await GetByIdAsync(rewindingProcess.Id);
    }

    /// <summary>
    /// Cập nhật công đoạn tua
    /// </summary>
    public async Task<RewindingProcess> UpdateAsync(int id, UpdateRewindingProcessDto dto)
    {
        var currentUserId = _userContextService.GetCurrentUserId()
            ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");
        var rewindingProcess = await _dbContext.RewindingProcesses
            .Include(rp => rp.Lines)
            .FirstOrDefaultAsync(rp => rp.Id == id);

        if (rewindingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn tua với ID: {id}");
        }

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
        rewindingProcess.ProductionDate = dto.ProductionDate;
        rewindingProcess.ProductionShift = dto.ProductionShift;
        rewindingProcess.Notes = dto.Notes;
        rewindingProcess.IsDraft = dto.IsDraft;
        rewindingProcess.ModifiedAt = DateTime.Now;
        rewindingProcess.ModifierId = currentUserId;
        rewindingProcess.ShiftLeaderId = dto.ShiftLeaderId;

        // Cập nhật lines
        UpdateLines(rewindingProcess, dto.Lines, existingProductionOrders);

        // Tính toán lại tổng
        CalculateTotals(rewindingProcess);

        var productOrderCompletedIds = rewindingProcess.Lines
            .Where(l => l.IsCompleted)
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToArray() ?? [];

        if (productOrderCompletedIds.Length > 0)
        {
            await _dbContext.UpdateStatusProductionOrderSapAsync("U_TUASTATUS", productOrderCompletedIds);
        }

        if (rewindingProcess.Lines.All(l => l.Status == 1))
        {
            rewindingProcess.Status = 1; // Hoàn thành
        }
        else if (rewindingProcess.Lines.Any(l => l.Status == 1))
        {
            rewindingProcess.Status = 2; // Đang tiến hành
        }
        else if (rewindingProcess.Lines.All(l => l.Status == 0))
        {
            rewindingProcess.Status = 0;
        }

        await _dbContext.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Xóa công đoạn tua
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var rewindingProcess = await _dbContext.RewindingProcesses
            .Include(rp => rp.Lines)
            .FirstOrDefaultAsync(rp => rp.Id == id);

        if (rewindingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn tua với ID: {id}");
        }

        _dbContext.RewindingProcesses.Remove(rewindingProcess);
        await _dbContext.SaveChangesAsync();
    }

    #region Private Methods

    private static RewindingProcessLine MapCreateToRewindingProcessLine(
        CreateRewindingProcessLineDto dto,
        string itemCode,
        string? itemName,
        string? cardCode,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? productTypeName,
        string? thickness,
        string? semiProductWidth
    )
    {
        if (dto.ExcessPO < 0)
        {
            dto.ExcessPO = 0;
        }
        var line = new RewindingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            ItemName = itemName,
            CardCode = cardCode,
            ProductType = productType,
            ProductTypeName = productTypeName,
            ProductionBatch = productionBatch,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            RewindingMachine = dto.RewindingMachine,
            RewindingMachineName = dto.RewindingMachineName,
            WorkerId = dto.WorkerId,
            RewindingSpeed = dto.RewindingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MachineStopMinutes = dto.MachineStopMinutes,
            StopReason = dto.StopReason,
            RollCount = dto.RollCount,
            QuantityKg = dto.QuantityKg,
            BoxCount = dto.BoxCount,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            BlowingLossKg = dto.BlowingLossKg,
            BlowingLossReason = dto.BlowingLossReason,
            HumanLossKg = dto.HumanLossKg,
            HumanLossReason = dto.HumanLossReason,
            MachineLossKg = dto.MachineLossKg,
            MachineLossReason = dto.MachineLossReason,
            ExcessPO = dto.ExcessPO,
            BtpWarehouseConfirmed = dto.BtpWarehouseConfirmed,
            Note = dto.Note
        };

        // Tính toán tổng DC cho line
        line.TotalLossKg = line.BlowingLossKg + line.HumanLossKg + line.MachineLossKg;

        return line;
    }

    private static RewindingProcessLine MapUpdateToRewindingProcessLine(
        UpdateRewindingProcessLineDto dto,
        string itemCode,
        string? itemName,
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
        if (dto.ExcessPO < 0)
        {
            dto.ExcessPO = 0;
        }
        var line = new RewindingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            ItemName = itemName,
            CardCode = cardCode,
            ProductionBatch = productionBatch,
            ProductType = productType,
            ProductTypeName = productTypeName,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            RewindingMachine = dto.RewindingMachine,
            RewindingMachineName = dto.RewindingMachineName,
            WorkerId = dto.WorkerId,
            RewindingSpeed = dto.RewindingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MachineStopMinutes = dto.MachineStopMinutes,
            StopReason = dto.StopReason,
            RollCount = dto.RollCount,
            QuantityKg = dto.QuantityKg,
            BoxCount = dto.BoxCount,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            BlowingLossKg = dto.BlowingLossKg,
            BlowingLossReason = dto.BlowingLossReason,
            HumanLossKg = dto.HumanLossKg,
            HumanLossReason = dto.HumanLossReason,
            MachineLossKg = dto.MachineLossKg,
            MachineLossReason = dto.MachineLossReason,
            ExcessPO = dto.ExcessPO,
            BtpWarehouseConfirmed = dto.BtpWarehouseConfirmed,
            Note = dto.Note
        };

        if (existingId.HasValue)
        {
            line.Id = existingId.Value;
        }

        // Tính toán tổng DC cho line
        line.TotalLossKg = line.BlowingLossKg + line.HumanLossKg + line.MachineLossKg;

        return line;
    }

    private void UpdateLines(
        RewindingProcess rewindingProcess,
        List<UpdateRewindingProcessLineDto> lineDtos,
        Dictionary<int, ProductionOrder> existingProductionOrders
    )
    {
        // Xóa các line không còn tồn tại trong DTO
        var dtoLineIds = lineDtos
            .Where(dto => dto.Id.HasValue)
            .Select(dto => dto.Id!.Value)
            .ToHashSet();

        var linesToRemove = rewindingProcess.Lines
            .Where(line => !dtoLineIds.Contains(line.Id))
            .ToList();

        foreach (var line in linesToRemove)
        {
            rewindingProcess.Lines.Remove(line);
            _dbContext.RewindingProcessLines.Remove(line);
        }

        // Cập nhật hoặc thêm mới các line
        foreach (var lineDto in lineDtos)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId)
                ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");

            var item = productionOrder.ItemDetail
                ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");

            if (lineDto.Id.HasValue)
            {
                // Cập nhật line hiện có
                var existingLine = rewindingProcess.Lines.FirstOrDefault(l => l.Id == lineDto.Id.Value);
                if (existingLine != null)
                {
                    var updatedLine = MapUpdateToRewindingProcessLine(
                        lineDto,
                        productionOrder.ItemCode,
                        productionOrder.ProdName,
                        productionOrder?.CardCode,
                        productionOrder?.ProductionBatch,
                        null, // RequiredDate sẽ lấy từ DTO hoặc để null
                        item.ProductType,
                        item.ProductTypeName,
                        item.Thickness,
                        item.SemiProductWidth,
                        lineDto.Id);

                    updatedLine.RewindingProcessId = existingLine.RewindingProcessId; // Giữ nguyên khóa ngoại
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapUpdateToRewindingProcessLine(
                    lineDto,
                    productionOrder.ItemCode,
                    productionOrder.ProdName,
                    productionOrder?.CardCode,
                    productionOrder?.ProductionBatch,
                    null, // RequiredDate sẽ lấy từ DTO hoặc để null
                    item.ProductType,
                    item.ProductTypeName,
                    item.Thickness,
                    item.SemiProductWidth);

                rewindingProcess.Lines.Add(newLine);
            }
        }
    }

    private static void CalculateTotals(RewindingProcess rewindingProcess)
    {
        rewindingProcess.TotalRewindingOutput = rewindingProcess.Lines.Sum(l => l.QuantityKg);
        rewindingProcess.TotalBlowingStageMold = rewindingProcess.Lines.Sum(l => l.BlowingLossKg);
        rewindingProcess.TotalRewindingStageMold = rewindingProcess.Lines.Sum(l => l.HumanLossKg + l.MachineLossKg);
    }

    #endregion
}