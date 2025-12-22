using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

/// <summary>
/// Service quản lý công đoạn pha hạt (Thổi)
/// </summary>
public class GrainMixingBlowingProcessService
{
    private readonly AppDbContext _dbContext;
    private readonly UserContextService _userContextService;

    public GrainMixingBlowingProcessService(
        AppDbContext dbContext,
        UserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn pha hạt (Thổi)
    /// </summary>
    public async Task<PaginationResponse<GrainMixingBlowingProcess>> GetAllAsync(QueryParam pr)
    {
        var query = _dbContext.GrainMixingBlowingProcesses
            .AsNoTracking()
            .ApplyFiltering(pr)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        if (pr.Include == "lines")
        {
            query = query
                    .Include(cp => cp.Lines)
                    .ThenInclude(line => line.BusinessPartner)
                    .Include(bp => bp.Lines)
                    .ThenInclude(bp => bp.Worker);
        }

        var result = await query
            .Include(gm => gm.Creator)
            .Include(gm => gm.Modifier)
            .OrderByDescending(gm => gm.Id)
            .ApplyOrderingAndPaging(pr)
            .ToListAsync();

        return new PaginationResponse<GrainMixingBlowingProcess>
        {
            Results = result,
            TotalCount = totalCount,
            PageSize = pr.PageSize,
            Page = pr.Page,
        };
    }

    /// <summary>
    /// Lấy công đoạn pha hạt (Thổi) theo ID
    /// </summary>
    public async Task<GrainMixingBlowingProcess> GetByIdAsync(int id)
    {
        var grainMixingBlowingProcess = await _dbContext.GrainMixingBlowingProcesses
            .AsNoTracking()
            .Include(gm => gm.Creator)
            .Include(gm => gm.Modifier)
            .Include(gm => gm.Lines)
            .ThenInclude(line => line.Worker)
            .Include(gm => gm.Lines)
            .ThenInclude(line => line.BusinessPartner)
            .FirstOrDefaultAsync(gm => gm.Id == id);

        if (grainMixingBlowingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn pha hạt (Thổi) với ID: {id}");
        }

        return grainMixingBlowingProcess;
    }

    /// <summary>
    /// Tạo công đoạn pha hạt (Thổi) mới
    /// </summary>
    public async Task<GrainMixingBlowingProcess> CreateAsync(CreateGrainMixingBlowingProcessDto dto)
    {
        var currentUserId = _userContextService.GetCurrentUserId()
            ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");

        // Validate workers if provided
        var workerIds = dto.Lines
            .Where(l => l.WorkerId.HasValue)
            .Select(l => l.WorkerId!.Value)
            .Distinct()
            .ToList();

        if (workerIds.Any())
        {
            var existingWorkers = await _dbContext.Employees
                .Where(e => workerIds.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync();

            var missingWorkerIds = workerIds.Except(existingWorkers).ToList();
            if (missingWorkerIds.Any())
            {
                throw new NotFoundException($"Không tìm thấy công nhân với ID: {string.Join(", ", missingWorkerIds)}");
            }
        }

        // Validate business partners if provided
        var cardCodes = dto.Lines
            .Where(l => !string.IsNullOrEmpty(l.CardCode))
            .Select(l => l.CardCode!)
            .Distinct()
            .ToList();

        if (cardCodes.Any())
        {
            var existingCardCodes = await _dbContext.BusinessPartners
                .Where(bp => cardCodes.Contains(bp.CardCode))
                .Select(bp => bp.CardCode)
                .ToListAsync();

            var missingCardCodes = cardCodes.Except(existingCardCodes).ToList();
            if (missingCardCodes.Any())
            {
                throw new NotFoundException($"Không tìm thấy khách hàng với mã: {string.Join(", ", missingCardCodes)}");
            }
        }

        var productionOrderIds = dto.Lines
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToList();

        var existingProductionOrders = await _dbContext.ProductionOrderGrainMixings.AsNoTracking()
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        var itemCodes = dto.Lines.Select(l => l.ItemCode).Distinct().ToList();

        var existingItems = await _dbContext.Items.AsNoTracking()
            .Where(i => itemCodes.Contains(i.ItemCode))
            .ToDictionaryAsync(i => i.ItemCode);

        if (existingItems == null)
        {
            throw new NotFoundException("Không tìm thấy Item nào phù hợp với các mã hàng đã cung cấp");
        }

        var lines = new List<GrainMixingBlowingProcessLine>();
        foreach (var lineDto in dto.Lines)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ??
                throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");

            var item = existingItems!.GetValueOrDefault(lineDto.ItemCode) ??
                throw new NotFoundException($"Không tìm thấy Item với mã: {lineDto.ItemCode}");

            var line = MapCreateToGrainMixingBlowingProcessLine(
                lineDto,
                item.ItemName,
                lineDto.ProductionOrderId,
                productionOrder.ProductionBatch.ToString() ?? string.Empty,
                productionOrder.DateOfNeed);

            lines.Add(line);
        }


        var grainMixingBlowingProcess = new GrainMixingBlowingProcess
        {
            CreatorId = currentUserId,
            ProductionShift = dto.ProductionShift,
            ProductionDate = dto.ProductionDate,
            IsDraft = dto.IsDraft,
            BlowingMachine = dto.BlowingMachine,
            BlowingMachineName = dto.BlowingMachineName,
            Notes = dto.Notes,
            Lines = lines
        };

        _dbContext.GrainMixingBlowingProcesses.Add(grainMixingBlowingProcess);
        await _dbContext.SaveChangesAsync();

        return await GetByIdAsync(grainMixingBlowingProcess.Id);
    }

    /// <summary>
    /// Cập nhật công đoạn pha hạt (Thổi)
    /// </summary>
    public async Task<GrainMixingBlowingProcess> UpdateAsync(int id, UpdateGrainMixingBlowingProcessDto dto)
    {
        var currentUserId = _userContextService.GetCurrentUserId()
            ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");

        var grainMixingBlowingProcess = await _dbContext.GrainMixingBlowingProcesses
            .Include(gm => gm.Lines)
            .FirstOrDefaultAsync(gm => gm.Id == id);

        if (grainMixingBlowingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn pha hạt (Thổi) với ID: {id}");
        }

        // Validate workers if provided
        var workerIds = dto.Lines
            .Where(l => l.WorkerId.HasValue)
            .Select(l => l.WorkerId!.Value)
            .Distinct()
            .ToList();

        if (workerIds.Any())
        {
            var existingWorkers = await _dbContext.Employees
                .Where(e => workerIds.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync();

            var missingWorkerIds = workerIds.Except(existingWorkers).ToList();
            if (missingWorkerIds.Any())
            {
                throw new NotFoundException($"Không tìm thấy công nhân với ID: {string.Join(", ", missingWorkerIds)}");
            }
        }

        // Validate business partners if provided
        var cardCodes = dto.Lines
            .Where(l => !string.IsNullOrEmpty(l.CardCode))
            .Select(l => l.CardCode!)
            .Distinct()
            .ToList();

        if (cardCodes.Any())
        {
            var existingCardCodes = await _dbContext.BusinessPartners
                .Where(bp => cardCodes.Contains(bp.CardCode))
                .Select(bp => bp.CardCode)
                .ToListAsync();

            var missingCardCodes = cardCodes.Except(existingCardCodes).ToList();
            if (missingCardCodes.Any())
            {
                throw new NotFoundException($"Không tìm thấy khách hàng với mã: {string.Join(", ", missingCardCodes)}");
            }
        }


        // Cập nhật thông tin cơ bản
        grainMixingBlowingProcess.ProductionDate = dto.ProductionDate;
        grainMixingBlowingProcess.ProductionShift = dto.ProductionShift;
        grainMixingBlowingProcess.IsDraft = dto.IsDraft;
        grainMixingBlowingProcess.IsDraft = dto.IsDraft;
        grainMixingBlowingProcess.BlowingMachine = dto.BlowingMachine;
        grainMixingBlowingProcess.BlowingMachineName = dto.BlowingMachineName;
        grainMixingBlowingProcess.Notes = dto.Notes;
        grainMixingBlowingProcess.ModifierId = currentUserId;
        grainMixingBlowingProcess.ModifiedAt = DateTime.Now;

        var productionOrderIds = dto.Lines
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToList();

        var existingProductionOrders = await _dbContext.ProductionOrderGrainMixings.AsNoTracking()
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        var itemCodes = dto.Lines.Select(l => l.ItemCode).Distinct().ToList();
        var existingItems = await _dbContext.Items.AsNoTracking()
            .Where(i => itemCodes.Contains(i.ItemCode))
            .ToDictionaryAsync(i => i.ItemCode);

        if (existingItems == null)
        {
            throw new NotFoundException("Không tìm thấy Item nào phù hợp với các mã hàng đã cung cấp");
        }

        // Cập nhật lines
        UpdateLines(grainMixingBlowingProcess, existingItems, dto.Lines, existingProductionOrders);

        if (!grainMixingBlowingProcess.IsDraft)
        {
            if (grainMixingBlowingProcess.Lines.All(l => l.Status == 1))
            {
                grainMixingBlowingProcess.Status = 1; // Hoàn thành
            }
            else if (grainMixingBlowingProcess.Lines.Any(l => l.Status == 1))
            {
                grainMixingBlowingProcess.Status = 2; // Đang tiến hành
            }
            else if (grainMixingBlowingProcess.Lines.All(l => l.Status == 0))
            {
                grainMixingBlowingProcess.Status = 0;
            }
        }

        await _dbContext.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Xóa công đoạn pha hạt (Thổi)
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var grainMixingBlowingProcess = await _dbContext.GrainMixingBlowingProcesses
            .Include(gm => gm.Lines)
            .FirstOrDefaultAsync(gm => gm.Id == id);

        if (grainMixingBlowingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn pha hạt (Thổi) với ID: {id}");
        }

        _dbContext.GrainMixingBlowingProcesses.Remove(grainMixingBlowingProcess);
        await _dbContext.SaveChangesAsync();
    }

    #region Private Methods

    private static GrainMixingBlowingProcessLine MapCreateToGrainMixingBlowingProcessLine(
        CreateGrainMixingBlowingProcessLineDto dto, string? itemName, int productOrderId, string productionBatch, DateTime? requiredDate)
    {
        return new GrainMixingBlowingProcessLine
        {
            ItemCode = dto.ItemCode,
            ItemName = itemName,
            ProductionOrderId = productOrderId,
            ProductionBatch = productionBatch,
            CardCode = dto.CardCode,
            MaterialIssueVoucherNo = dto.MaterialIssueVoucherNo,
            MixtureType = dto.MixtureType,
            Specification = dto.Specification,
            WorkerId = dto.WorkerId,
            MachineName = dto.MachineName,
            MixingMachine = dto.MixingMachine,
            MixingMachineName = dto.MixingMachineName,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            // PP
            PpTron = dto.PpTron,
            PpHdNhot = dto.PpHdNhot,
            PpLdpe = dto.PpLdpe,
            PpDc = dto.PpDc,
            PpAdditive = dto.PpAdditive,
            PpColor = dto.PpColor,
            PpOther = dto.PpOther,
            PpRit = dto.PpRit,
            // HD
            HdLldpe2320 = dto.HdLldpe2320,
            HdRecycled = dto.HdRecycled,
            HdTalcol = dto.HdTalcol,
            HdDc = dto.HdDc,
            HdColor = dto.HdColor,
            HdOther = dto.HdOther,
            HdHd = dto.HdHd,
            // PE
            PeAdditive = dto.PeAdditive,
            PeTalcol = dto.PeTalcol,
            PeColor = dto.PeColor,
            PeOther = dto.PeOther,
            PeLdpe = dto.PeLdpe,
            PeLldpe = dto.PeLldpe,
            PeRecycled = dto.PeRecycled,
            PeDc = dto.PeDc,
            // Màng co
            ShrinkRe707 = dto.ShrinkRe707,
            ShrinkSlip = dto.ShrinkSlip,
            ShrinkStatic = dto.ShrinkStatic,
            ShrinkDc = dto.ShrinkDc,
            ShrinkTalcol = dto.ShrinkTalcol,
            ShrinkOther = dto.ShrinkOther,
            ShrinkLldpe = dto.ShrinkLldpe,
            ShrinkLdpe = dto.ShrinkLdpe,
            ShrinkRecycled = dto.ShrinkRecycled,
            ShrinkTangDai = dto.ShrinkTangDai,
            // Màng chít
            WrapRecycledCa = dto.WrapRecycledCa,
            WrapRecycledCb = dto.WrapRecycledCb,
            WrapGlue = dto.WrapGlue,
            WrapColor = dto.WrapColor,
            WrapDc = dto.WrapDc,
            WrapLdpe = dto.WrapLdpe,
            WrapLldpe = dto.WrapLldpe,
            WrapSlip = dto.WrapSlip,
            WrapAdditive = dto.WrapAdditive,
            WrapOther = dto.WrapOther,
            WrapTangDaiC6 = dto.WrapTangDaiC6,
            WrapTangDaiC8 = dto.WrapTangDaiC8,
            // EVA
            EvaPop3070 = dto.EvaPop3070,
            EvaLdpe = dto.EvaLdpe,
            EvaDc = dto.EvaDc,
            EvaTalcol = dto.EvaTalcol,
            EvaSlip = dto.EvaSlip,
            EvaStaticAdditive = dto.EvaStaticAdditive,
            EvaOther = dto.EvaOther,
            EvaTgc = dto.EvaTgc,
            // Other fields
            QuantityKg = dto.QuantityKg,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            Note = dto.Note
        };
    }

    private static GrainMixingBlowingProcessLine MapUpdateToGrainMixingBlowingProcessLine(
        UpdateGrainMixingBlowingProcessLineDto dto,
        string? itemName,
        int productionOrderId,
        string productionBatch,
        DateTime? requiredDate,
        int? existingId = null)
    {
        var line = new GrainMixingBlowingProcessLine
        {
            ItemCode = dto.ItemCode,
            ItemName = itemName,
            ProductionOrderId = productionOrderId,
            ProductionBatch = productionBatch,
            CardCode = dto.CardCode,
            MaterialIssueVoucherNo = dto.MaterialIssueVoucherNo,
            MixtureType = dto.MixtureType,
            Specification = dto.Specification,
            WorkerId = dto.WorkerId,
            MachineName = dto.MachineName,
            MixingMachine = dto.MixingMachine,
            MixingMachineName = dto.MixingMachineName,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            // PP
            PpTron = dto.PpTron,
            PpHdNhot = dto.PpHdNhot,
            PpLdpe = dto.PpLdpe,
            PpDc = dto.PpDc,
            PpAdditive = dto.PpAdditive,
            PpColor = dto.PpColor,
            PpOther = dto.PpOther,
            PpRit = dto.PpRit,
            // HD
            HdLldpe2320 = dto.HdLldpe2320,
            HdRecycled = dto.HdRecycled,
            HdTalcol = dto.HdTalcol,
            HdDc = dto.HdDc,
            HdColor = dto.HdColor,
            HdOther = dto.HdOther,
            HdHd = dto.HdHd,
            // PE
            PeAdditive = dto.PeAdditive,
            PeTalcol = dto.PeTalcol,
            PeColor = dto.PeColor,
            PeOther = dto.PeOther,
            PeLdpe = dto.PeLdpe,
            PeLldpe = dto.PeLldpe,
            PeRecycled = dto.PeRecycled,
            PeDc = dto.PeDc,
            // Màng co
            ShrinkRe707 = dto.ShrinkRe707,
            ShrinkSlip = dto.ShrinkSlip,
            ShrinkStatic = dto.ShrinkStatic,
            ShrinkDc = dto.ShrinkDc,
            ShrinkLdpe = dto.ShrinkLdpe,
            ShrinkLldpe = dto.ShrinkLldpe,
            ShrinkRecycled = dto.ShrinkRecycled,
            ShrinkTangDai = dto.ShrinkTangDai,
            ShrinkTalcol = dto.ShrinkTalcol,
            ShrinkOther = dto.ShrinkOther,
            // Màng chít
            WrapRecycledCa = dto.WrapRecycledCa,
            WrapRecycledCb = dto.WrapRecycledCb,
            WrapGlue = dto.WrapGlue,
            WrapColor = dto.WrapColor,
            WrapDc = dto.WrapDc,
            WrapLdpe = dto.WrapLdpe,
            WrapLldpe = dto.WrapLldpe,
            WrapSlip = dto.WrapSlip,
            WrapAdditive = dto.WrapAdditive,
            WrapOther = dto.WrapOther,
            WrapTangDaiC6 = dto.WrapTangDaiC6,
            WrapTangDaiC8 = dto.WrapTangDaiC8,
            // EVA
            EvaPop3070 = dto.EvaPop3070,
            EvaLdpe = dto.EvaLdpe,
            EvaDc = dto.EvaDc,
            EvaTalcol = dto.EvaTalcol,
            EvaSlip = dto.EvaSlip,
            EvaStaticAdditive = dto.EvaStaticAdditive,
            EvaOther = dto.EvaOther,
            EvaTgc = dto.EvaTgc,
            // Other fields
            QuantityKg = dto.QuantityKg,
            RequiredDate = requiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason,
            Note = dto.Note
        };

        if (existingId.HasValue)
        {
            line.Id = existingId.Value;
        }

        return line;
    }

    private void UpdateLines(
        GrainMixingBlowingProcess grainMixingBlowingProcess,
        Dictionary<string, Item>? items,
        List<UpdateGrainMixingBlowingProcessLineDto> lineDtos,
        Dictionary<int, ProductionOrderGrainMixing> existingProductionOrders
    )
    {
        // Xóa các line không còn tồn tại trong DTO
        var dtoLineIds = lineDtos
            .Where(dto => dto.Id.HasValue)
            .Select(dto => dto.Id!.Value)
            .ToHashSet();

        var linesToRemove = grainMixingBlowingProcess.Lines
            .Where(line => !dtoLineIds.Contains(line.Id))
            .ToList();

        foreach (var line in linesToRemove)
        {
            grainMixingBlowingProcess.Lines.Remove(line);
            _dbContext.GrainMixingBlowingProcessLines.Remove(line);
        }

        // Cập nhật hoặc thêm mới các line
        foreach (var lineDto in lineDtos)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ??
                throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");
            
            var item = items!.GetValueOrDefault(lineDto.ItemCode) ??
                throw new NotFoundException($"Không tìm thấy Item với mã: {lineDto.ItemCode}");
            if (lineDto.Id.HasValue)
            {
                // Cập nhật line hiện có
                var existingLine = grainMixingBlowingProcess.Lines
                    .FirstOrDefault(l => l.Id == lineDto.Id.Value);

                if (existingLine != null)
                {
                    var updatedLine = MapUpdateToGrainMixingBlowingProcessLine(lineDto, item.ItemName, productionOrder.DocEntry,
                        productionOrder.ProductionBatch.ToString() ?? "", productionOrder.DateOfNeed, lineDto.Id);
                    updatedLine.GrainMixingBlowingProcessId = existingLine.GrainMixingBlowingProcessId;
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapUpdateToGrainMixingBlowingProcessLine(lineDto, item.ItemName, productionOrder.DocEntry,
                    productionOrder.ProductionBatch.ToString() ?? "", productionOrder.DateOfNeed);
                grainMixingBlowingProcess.Lines.Add(newLine);
            }
        }
    }

    #endregion
}