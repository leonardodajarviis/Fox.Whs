using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

/// <summary>
/// Service quản lý công đoạn pha hạt
/// </summary>
public class GrainMixingProcessService
{
    private readonly AppDbContext _dbContext;
    private readonly UserContextService _userContextService;

    public GrainMixingProcessService(
        AppDbContext dbContext,
        UserContextService userContextService)
    {
        _dbContext = dbContext;
        _userContextService = userContextService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn pha hạt
    /// </summary>
    public async Task<PaginationResponse<GrainMixingProcess>> GetAllAsync(QueryParam pr)
    {
        var query = _dbContext.GrainMixingProcesses
            .AsNoTracking()
            .ApplyFiltering(pr)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        if (pr.Include == "lines")
        {
            query = query.Include(bp => bp.Lines).ThenInclude(x => x.Worker);
        }

        var result = await query
            .Include(gm => gm.Creator)
            .Include(gm => gm.Modifier)
            .OrderByDescending(gm => gm.Id)
            .ApplyOrderingAndPaging(pr)
            .ToListAsync();

        return new PaginationResponse<GrainMixingProcess>
        {
            Results = result,
            TotalCount = totalCount,
            PageSize = pr.PageSize,
            Page = pr.Page,
        };
    }

    /// <summary>
    /// Lấy công đoạn pha hạt theo ID
    /// </summary>
    public async Task<GrainMixingProcess> GetByIdAsync(int id)
    {
        var grainMixingProcess = await _dbContext.GrainMixingProcesses
            .AsNoTracking()
            .Include(gm => gm.Creator)
            .Include(gm => gm.Modifier)
            .Include(gm => gm.Lines)
            .ThenInclude(line => line.Worker)
            .Include(gm => gm.Lines)
            .ThenInclude(line => line.BusinessPartner)
            .FirstOrDefaultAsync(gm => gm.Id == id);

        if (grainMixingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn pha hạt với ID: {id}");
        }

        return grainMixingProcess;
    }

    /// <summary>
    /// Tạo công đoạn pha hạt mới
    /// </summary>
    public async Task<GrainMixingProcess> CreateAsync(CreateGrainMixingProcessDto dto)
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

        var existingProductionOrders = await _dbContext.ProductionOrderGrainMixings
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        var lines = new List<GrainMixingProcessLine>();
        foreach (var lineDto in dto.Lines)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ??
                throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");

            var line = MapCreateToGrainMixingProcessLine(
                lineDto,
                lineDto.ProductionOrderId,
                productionOrder.ProductionBatch.ToString() ?? string.Empty,
                productionOrder.DateOfNeed);

            lines.Add(line);
        }

        var grainMixingProcess = new GrainMixingProcess
        {
            CreatorId = currentUserId,
            ProductionDate = dto.ProductionDate,
            IsDraft = dto.IsDraft,
            WorkerCount = dto.WorkerCount,
            Notes = dto.Notes,
            TotalHoursWorked = dto.TotalHoursWorked,
            Lines = lines
        };

        // Tính toán năng suất lao động
        CalculateLaborProductivity(grainMixingProcess);

        _dbContext.GrainMixingProcesses.Add(grainMixingProcess);
        await _dbContext.SaveChangesAsync();

        return await GetByIdAsync(grainMixingProcess.Id);
    }

    /// <summary>
    /// Cập nhật công đoạn pha hạt
    /// </summary>
    public async Task<GrainMixingProcess> UpdateAsync(int id, UpdateGrainMixingProcessDto dto)
    {
        var currentUserId = _userContextService.GetCurrentUserId()
            ?? throw new UnauthorizedException("Không xác định được người dùng hiện tại");

        var grainMixingProcess = await _dbContext.GrainMixingProcesses
            .Include(gm => gm.Lines)
            .FirstOrDefaultAsync(gm => gm.Id == id);

        if (grainMixingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn pha hạt với ID: {id}");
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
        grainMixingProcess.ProductionDate = dto.ProductionDate;
        grainMixingProcess.IsDraft = dto.IsDraft;
        grainMixingProcess.WorkerCount = dto.WorkerCount;
        grainMixingProcess.TotalHoursWorked = dto.TotalHoursWorked;
        grainMixingProcess.ModifierId = currentUserId;
        grainMixingProcess.Notes = dto.Notes;
        grainMixingProcess.ModifiedAt = DateTime.Now;

        var productionOrderIds = dto.Lines
            .Select(l => l.ProductionOrderId)
            .Distinct()
            .ToList();

        var existingProductionOrders = await _dbContext.ProductionOrderGrainMixings.AsNoTracking()
            .Where(po => productionOrderIds.Contains(po.DocEntry))
            .ToDictionaryAsync(po => po.DocEntry);

        // Cập nhật lines
        UpdateLines(grainMixingProcess, dto.Lines, existingProductionOrders);

        // Tính toán lại năng suất lao động
        CalculateLaborProductivity(grainMixingProcess);

        if (!grainMixingProcess.IsDraft)
        {
            if (grainMixingProcess.Lines.All(l => l.Status == 1))
            {
                grainMixingProcess.Status = 1; // Hoàn thành
            }
            else if (grainMixingProcess.Lines.Any(l => l.Status == 1))
            {
                grainMixingProcess.Status = 2; // Đang tiến hành
            }
            else if (grainMixingProcess.Lines.All(l => l.Status == 0))
            {
                grainMixingProcess.Status = 0;
            }
        }

        await _dbContext.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Xóa công đoạn pha hạt
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var grainMixingProcess = await _dbContext.GrainMixingProcesses
            .Include(gm => gm.Lines)
            .FirstOrDefaultAsync(gm => gm.Id == id);

        if (grainMixingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn pha hạt với ID: {id}");
        }

        _dbContext.GrainMixingProcesses.Remove(grainMixingProcess);
        await _dbContext.SaveChangesAsync();
    }

    #region Private Methods

    private static GrainMixingProcessLine MapCreateToGrainMixingProcessLine(
        CreateGrainMixingProcessLineDto dto, int productOrderId, string productionBatch, DateTime? requiredDate)
    {
        return new GrainMixingProcessLine
        {
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

    private static GrainMixingProcessLine MapUpdateToGrainMixingProcessLine(
        UpdateGrainMixingProcessLineDto dto,
        int productionOrderId,
        string productionBatch,
        DateTime? requiredDate,
        int? existingId = null)
    {
        var line = new GrainMixingProcessLine
        {
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

        if (existingId.HasValue)
        {
            line.Id = existingId.Value;
        }

        return line;
    }

    private void UpdateLines(
        GrainMixingProcess grainMixingProcess,
        List<UpdateGrainMixingProcessLineDto> lineDtos,
        Dictionary<int, ProductionOrderGrainMixing> existingProductionOrders
    )
    {
        // Xóa các line không còn tồn tại trong DTO
        var dtoLineIds = lineDtos
            .Where(dto => dto.Id.HasValue)
            .Select(dto => dto.Id!.Value)
            .ToHashSet();

        var linesToRemove = grainMixingProcess.Lines
            .Where(line => !dtoLineIds.Contains(line.Id))
            .ToList();

        foreach (var line in linesToRemove)
        {
            grainMixingProcess.Lines.Remove(line);
            _dbContext.GrainMixingProcessLines.Remove(line);
        }

        // Cập nhật hoặc thêm mới các line
        foreach (var lineDto in lineDtos)
        {
            var productionOrder = existingProductionOrders.GetValueOrDefault(lineDto.ProductionOrderId) ??
                throw new NotFoundException($"Không tìm thấy Production Order với ID: {lineDto.ProductionOrderId}");
            if (lineDto.Id.HasValue)
            {
                // Cập nhật line hiện có
                var existingLine = grainMixingProcess.Lines
                    .FirstOrDefault(l => l.Id == lineDto.Id.Value);

                if (existingLine != null)
                {
                    var updatedLine = MapUpdateToGrainMixingProcessLine(lineDto, productionOrder.DocEntry,
                        productionOrder.ProductionBatch.ToString() ?? "", productionOrder.DateOfNeed, lineDto.Id);
                    updatedLine.GrainMixingProcessId = existingLine.GrainMixingProcessId;
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapUpdateToGrainMixingProcessLine(lineDto, productionOrder.DocEntry,
                    productionOrder.ProductionBatch.ToString() ?? "", productionOrder.DateOfNeed);
                grainMixingProcess.Lines.Add(newLine);
            }
        }
    }

    private static void CalculateLaborProductivity(GrainMixingProcess grainMixingProcess)
    {
        var totalQuantity = grainMixingProcess.Lines.Sum(l => l.QuantityKg);
        var totalHours = grainMixingProcess.TotalHoursWorked;

        if (totalHours > 0 && grainMixingProcess.WorkerCount > 0)
        {
            grainMixingProcess.LaborProductivity = totalQuantity / (totalHours * grainMixingProcess.WorkerCount);
        }
        else
        {
            grainMixingProcess.LaborProductivity = 0;
        }
    }

    #endregion
}