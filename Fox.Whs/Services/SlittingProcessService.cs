using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

/// <summary>
/// Service quản lý công đoạn chia
/// </summary>
public class SlittingProcessService
{
    private readonly AppDbContext _dbContext;
    private readonly UserContextService _userContextService;

    public SlittingProcessService(
        AppDbContext dbContext,
        UserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn chia
    /// </summary>
    public async Task<PaginationResponse<SlittingProcess>> GetAllAsync(QueryParam pr)
    {
        var query = _dbContext.SlittingProcesses.AsNoTracking().ApplyFiltering(pr).AsQueryable();

        var totalCount = await query.CountAsync();

        if (pr.Include == "lines")
        {
            query = query.Include(bp => bp.Lines).ThenInclude(x => x.Worker);
        }

        var result = await query
            .Include(sp => sp.Creator)
            .Include(sp => sp.Modifier)
            .Include(sp => sp.ShiftLeader)
            .OrderByDescending(sp => sp.Id)
            .ApplyOrderingAndPaging(pr)
            .ToListAsync();

        return new PaginationResponse<SlittingProcess>
        {
            Results = result,
            TotalCount = totalCount,
            PageSize = pr.PageSize,
            Page = pr.Page,
        };
    }

    public async Task<SlittingProcess> GetByIdAsync(int id)
    {
        var slittingProcess = await _dbContext.SlittingProcesses
            .Include(sp => sp.Creator)
            .Include(sp => sp.Modifier)
            .Include(sp => sp.ShiftLeader)
            .Include(sp => sp.Lines)
                .ThenInclude(line => line.Worker)
            .FirstOrDefaultAsync(sp => sp.Id == id);

        if (slittingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn chia với ID: {id}");
        }

        return slittingProcess;
    }

    /// <summary>
    /// Tạo công đoạn chia mới
    /// </summary>
    public async Task<SlittingProcess> CreateAsync(CreateSlittingProcessDto dto)
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
            .Include(po => po.BusinessPartnerDetail)
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        var lines = new List<SlittingProcessLine>();
        foreach (var lineDto in dto.Lines)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId)
                ?? throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");

            var item = productionOrder.ItemDetail
                ?? throw new NotFoundException($"Không tìm thấy Item với mã: {productionOrder.ItemCode}");

            var line = MapCreateToSlittingProcessLine(
                lineDto,
                productionOrder.ItemCode,
                productionOrder.ProdName,
                productionOrder?.CardCode,
                productionOrder?.CustomerName,
                productionOrder?.ProductionBatch,
                productionOrder?.DateOfNeedSlitting,
                item.ProductType,
                item.ProductTypeName,
                item.Thickness,
                item.SemiProductWidth,
                item.PrintPatternName,
                item.ColorCount);

            lines.Add(line);
        }

        var slittingProcess = new SlittingProcess
        {
            ShiftLeaderId = shiftLeaderId,
            CreatorId = currentUserId,
            ProductionDate = dto.ProductionDate,
            IsDraft = dto.IsDraft,
            ProductionShift = dto.ProductionShift,
            Lines = lines
        };

        // Tính toán tổng
        CalculateTotals(slittingProcess);

        _dbContext.SlittingProcesses.Add(slittingProcess);
        await _dbContext.SaveChangesAsync();

        return await GetByIdAsync(slittingProcess.Id);
    }

    /// <summary>
    /// Cập nhật công đoạn chia
    /// </summary>
    public async Task<SlittingProcess> UpdateAsync(int id, UpdateSlittingProcessDto dto)
    {
        var currentUserId = _userContextService.GetCurrentUserId()
            ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");

        var slittingProcess = await _dbContext.SlittingProcesses
            .Include(sp => sp.Lines)
            .FirstOrDefaultAsync(sp => sp.Id == id);

        if (slittingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn chia với ID: {id}");
        }

        var productionOrderIds = dto.Lines
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToList();

        var existingProductionOrders = await _dbContext.ProductionOrders
            .Include(po => po.ItemDetail)
            .ThenInclude(po => po!.ProductTypeInfo)
            .Include(po => po.BusinessPartnerDetail)
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        // Cập nhật thông tin cơ bản
        slittingProcess.ProductionDate = dto.ProductionDate;
        slittingProcess.ProductionShift = dto.ProductionShift;
        slittingProcess.IsDraft = dto.IsDraft;
        slittingProcess.ModifierId = currentUserId;
        slittingProcess.ModifiedAt = DateTime.Now;
        slittingProcess.ShiftLeaderId = dto.ShiftLeaderId;

        // Cập nhật lines
        UpdateLines(slittingProcess, dto.Lines, existingProductionOrders);

        // Tính toán lại tổng
        CalculateTotals(slittingProcess);

        var productOrderCompletedIds = slittingProcess.Lines
            .Where(l => l.IsCompleted)
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToArray() ?? [];

        if (productOrderCompletedIds.Length > 0)
        {
            await _dbContext.UpdateStatusProductionOrderSapAsync("U_CHIASTATUS", productOrderCompletedIds);
        }

        if (slittingProcess.Lines.All(l => l.Status == 1))
        {
            slittingProcess.Status = 1; // Hoàn thành
        }
        else if (slittingProcess.Lines.Any(l => l.Status == 1))
        {
            slittingProcess.Status = 2; // Đang tiến hành
        }
        else if (slittingProcess.Lines.All(l => l.Status == 0))
        {
            slittingProcess.Status = 0;
        }

        await _dbContext.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Xóa công đoạn chia
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var slittingProcess = await _dbContext.SlittingProcesses
            .Include(sp => sp.Lines)
            .FirstOrDefaultAsync(sp => sp.Id == id);

        if (slittingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn chia với ID: {id}");
        }

        _dbContext.SlittingProcesses.Remove(slittingProcess);
        await _dbContext.SaveChangesAsync();
    }

    #region Private Methods

    private static SlittingProcessLine MapCreateToSlittingProcessLine(
        CreateSlittingProcessLineDto dto,
        string itemCode,
        string? itemName,
        string? cardCode,
        string? customerName,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? productTypeName,
        string? thickness,
        string? semiProductWidth,
        string? printPatternName,
        string? colorCount
    )
    {
        if (dto.ExcessPOPrinting < 0)
        {
            dto.ExcessPOPrinting = 0;
        }
        if (dto.ExcessPOSlitting < 0)
        {
            dto.ExcessPOSlitting = 0;
        }
        var line = new SlittingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            ItemName = itemName,
            CardCode = cardCode,
            CustomerName = customerName,
            ProductType = productType,
            ProductTypeName = productTypeName,
            ProductionBatch = productionBatch,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            PrintPatternName = printPatternName,
            ColorCount = colorCount,
            SlittingMachine = dto.SlittingMachine,
            SlittingMachineName = dto.SlittingMachineName,
            WorkerId = dto.WorkerId,
            SlittingSpeed = dto.SlittingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MachineStopMinutes = dto.MachineStopMinutes,
            StopReason = dto.StopReason,
            RollCount = dto.RollCount,
            PieceCount = dto.PieceCount,
            QuantityKg = dto.QuantityKg,
            BoxCount = dto.BoxCount,
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
            PrintingMachineName = dto.PrintingMachineName,
            CutViaKg = dto.CutViaKg,
            HumanLossKg = dto.HumanLossKg,
            HumanLossReason = dto.HumanLossReason,
            MachineLossKg = dto.MachineLossKg,
            MachineLossReason = dto.MachineLossReason,
            ExcessPOPrinting = dto.ExcessPOPrinting,
            ExcessPOSlitting = dto.ExcessPOSlitting,
            BtpWarehouseConfirmed = dto.BtpWarehouseConfirmed,
            Note = dto.Note
        };

        // Tính toán tổng DC cho line
        line.TotalLossKg = line.ProcessingLossKg + line.BlowingLossKg +
                          line.PrintingLossKg + line.CutViaKg +
                          line.HumanLossKg + line.MachineLossKg;

        return line;
    }

    private static SlittingProcessLine MapUpdateToSlittingProcessLine(
        UpdateSlittingProcessLineDto dto,
        string itemCode,
        string? itemName,
        string? cardCode,
        string? customerName,
        string? productionBatch,
        DateTime? requiredDate,
        string? productType,
        string? productTypeName,
        string? thickness,
        string? semiProductWidth,
        string? printPatternName,
        string? colorCount,
        int? existingId = null
    )
    {
        if (dto.ExcessPOPrinting < 0)
        {
            dto.ExcessPOPrinting = 0;
        }
        if (dto.ExcessPOSlitting < 0)
        {
            dto.ExcessPOSlitting = 0;
        }
        var line = new SlittingProcessLine
        {
            ProductionOrderId = dto.ProductionOrderId,
            ItemCode = itemCode,
            ItemName = itemName,
            CardCode = cardCode,
            CustomerName = customerName,
            ProductionBatch = productionBatch,
            ProductType = productType,
            ProductTypeName = productTypeName,
            Thickness = thickness,
            SemiProductWidth = semiProductWidth,
            PrintPatternName = printPatternName,
            ColorCount = colorCount,
            SlittingMachine = dto.SlittingMachine,
            SlittingMachineName = dto.SlittingMachineName,
            WorkerId = dto.WorkerId,
            SlittingSpeed = dto.SlittingSpeed,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            MachineStopMinutes = dto.MachineStopMinutes,
            StopReason = dto.StopReason,
            RollCount = dto.RollCount,
            PieceCount = dto.PieceCount,
            QuantityKg = dto.QuantityKg,
            BoxCount = dto.BoxCount,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            ProcessingLossKg = dto.ProcessingLossKg,
            ProcessingLossReason = dto.ProcessingLossReason,
            BlowingLossKg = dto.BlowingLossKg,
            BlowingLossReason = dto.BlowingLossReason,
            PrintingLossKg = dto.PrintingLossKg,
            PrintingLossReason = dto.PrintingLossReason,
            PrintingMachine = dto.PrintingMachine,
            PrintingMachineName = dto.PrintingMachineName,
            CutViaKg = dto.CutViaKg,
            HumanLossKg = dto.HumanLossKg,
            HumanLossReason = dto.HumanLossReason,
            MachineLossKg = dto.MachineLossKg,
            MachineLossReason = dto.MachineLossReason,
            ExcessPOPrinting = dto.ExcessPOPrinting,
            ExcessPOSlitting = dto.ExcessPOSlitting,
            BtpWarehouseConfirmed = dto.BtpWarehouseConfirmed,
            Note = dto.Note
        };

        if (existingId.HasValue)
        {
            line.Id = existingId.Value;
        }

        // Tính toán tổng DC cho line
        line.TotalLossKg = line.ProcessingLossKg + line.BlowingLossKg +
                          line.PrintingLossKg + line.CutViaKg +
                          line.HumanLossKg + line.MachineLossKg;

        return line;
    }

    private void UpdateLines(
        SlittingProcess slittingProcess,
        List<UpdateSlittingProcessLineDto> lineDtos,
        Dictionary<int, ProductionOrder> existingProductionOrders
    )
    {
        // Xóa các line không còn tồn tại trong DTO
        var dtoLineIds = lineDtos
            .Where(dto => dto.Id.HasValue)
            .Select(dto => dto.Id!.Value)
            .ToHashSet();

        var linesToRemove = slittingProcess.Lines
            .Where(line => !dtoLineIds.Contains(line.Id))
            .ToList();

        foreach (var line in linesToRemove)
        {
            slittingProcess.Lines.Remove(line);
            _dbContext.SlittingProcessLines.Remove(line);
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
                var existingLine = slittingProcess.Lines.FirstOrDefault(l => l.Id == lineDto.Id.Value);
                if (existingLine != null)
                {
                    var updatedLine = MapUpdateToSlittingProcessLine(
                        lineDto,
                        productionOrder.ItemCode,
                        productionOrder.ProdName,
                        productionOrder?.CardCode,
                        productionOrder?.CustomerName,
                        productionOrder?.ProductionBatch,
                        productionOrder?.DateOfNeedSlitting,
                        item.ProductType,
                        item.ProductTypeName,
                        item.Thickness,
                        item.SemiProductWidth,
                        item.PrintPatternName,
                        item.ColorCount,
                        lineDto.Id);

                    updatedLine.SlittingProcessId = existingLine.SlittingProcessId;
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapUpdateToSlittingProcessLine(
                    lineDto,
                    productionOrder.ItemCode,
                    productionOrder.ProdName,
                    productionOrder?.CardCode,
                    productionOrder?.CustomerName,
                    productionOrder?.ProductionBatch,
                    productionOrder?.DateOfNeedSlitting,
                    item.ProductType,
                    item.ProductTypeName,
                    item.Thickness,
                    item.SemiProductWidth,
                    item.PrintPatternName,
                    item.ColorCount);

                slittingProcess.Lines.Add(newLine);
            }
        }
    }

    private static void CalculateTotals(SlittingProcess slittingProcess)
    {
        slittingProcess.TotalSlittingOutput = slittingProcess.Lines.Sum(l => l.QuantityKg);
        slittingProcess.TotalProcessingMold = slittingProcess.Lines.Sum(l => l.ProcessingLossKg);
        slittingProcess.TotalBlowingStageMold = slittingProcess.Lines.Sum(l => l.BlowingLossKg);
        slittingProcess.TotalPrintingStageMold = slittingProcess.Lines.Sum(l => l.PrintingLossKg);
        slittingProcess.TotalSlittingStageMold = slittingProcess.Lines.Sum(l => l.HumanLossKg + l.MachineLossKg + l.CutViaKg);
    }

    #endregion
}
