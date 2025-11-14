using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
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

        var result = await query
            .Include(gm => gm.Creator)
            .Include(gm => gm.Modifier)
            .ApplyOrderingAndPaging(pr)
            .OrderByDescending(gm => gm.ProductionDate)
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

        var lines = dto.Lines.Select(MapCreateToGrainMixingBlowingProcessLine).ToList();

        var grainMixingBlowingProcess = new GrainMixingBlowingProcess
        {
            CreatorId = currentUserId,
            ProductionDate = dto.ProductionDate,
            IsDraft = dto.IsDraft,
            BlowingMachine = dto.BlowingMachine,
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
        grainMixingBlowingProcess.IsDraft = dto.IsDraft;
        grainMixingBlowingProcess.BlowingMachine = dto.BlowingMachine;
        grainMixingBlowingProcess.ModifierId = currentUserId;
        grainMixingBlowingProcess.ModifiedAt = DateTime.Now;

        // Cập nhật lines
        UpdateLines(grainMixingBlowingProcess, dto.Lines);

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
        CreateGrainMixingBlowingProcessLineDto dto)
    {
        return new GrainMixingBlowingProcessLine
        {
            ProductionBatch = dto.ProductionBatch,
            CardCode = dto.CardCode,
            MaterialIssueVoucherNo = dto.MaterialIssueVoucherNo,
            MixtureType = dto.MixtureType,
            Specification = dto.Specification,
            WorkerId = dto.WorkerId,
            MachineName = dto.MachineName,
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
            // HD
            HdLldpe2320 = dto.HdLldpe2320,
            HdRecycled = dto.HdRecycled,
            HdTalcol = dto.HdTalcol,
            HdDc = dto.HdDc,
            HdColor = dto.HdColor,
            HdOther = dto.HdOther,
            // PE
            PeAdditive = dto.PeAdditive,
            PeTalcol = dto.PeTalcol,
            PeColor = dto.PeColor,
            PeOther = dto.PeOther,
            PeLdpe = dto.PeLdpe,
            PeLldpe = dto.PeLldpe,
            PeRecycled = dto.PeRecycled,
            // Màng co
            ShrinkRe707 = dto.ShrinkRe707,
            ShrinkSlip = dto.ShrinkSlip,
            ShrinkStatic = dto.ShrinkStatic,
            ShrinkDc = dto.ShrinkDc,
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
            // EVA
            EvaPop3070 = dto.EvaPop3070,
            EvaLdpe = dto.EvaLdpe,
            EvaDc = dto.EvaDc,
            EvaTalcol = dto.EvaTalcol,
            EvaSlip = dto.EvaSlip,
            EvaStaticAdditive = dto.EvaStaticAdditive,
            EvaOther = dto.EvaOther,
            // Other fields
            QuantityKg = dto.QuantityKg,
            RequiredDate = dto.RequiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason
        };
    }

    private static GrainMixingBlowingProcessLine MapUpdateToGrainMixingBlowingProcessLine(
        UpdateGrainMixingBlowingProcessLineDto dto,
        int? existingId = null)
    {
        var line = new GrainMixingBlowingProcessLine
        {
            ProductionBatch = dto.ProductionBatch,
            CardCode = dto.CardCode,
            MaterialIssueVoucherNo = dto.MaterialIssueVoucherNo,
            MixtureType = dto.MixtureType,
            Specification = dto.Specification,
            WorkerId = dto.WorkerId,
            MachineName = dto.MachineName,
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
            // HD
            HdLldpe2320 = dto.HdLldpe2320,
            HdRecycled = dto.HdRecycled,
            HdTalcol = dto.HdTalcol,
            HdDc = dto.HdDc,
            HdColor = dto.HdColor,
            HdOther = dto.HdOther,
            // PE
            PeAdditive = dto.PeAdditive,
            PeTalcol = dto.PeTalcol,
            PeColor = dto.PeColor,
            PeOther = dto.PeOther,
            PeLdpe = dto.PeLdpe,
            PeLldpe = dto.PeLldpe,
            PeRecycled = dto.PeRecycled,
            // Màng co
            ShrinkRe707 = dto.ShrinkRe707,
            ShrinkSlip = dto.ShrinkSlip,
            ShrinkStatic = dto.ShrinkStatic,
            ShrinkDc = dto.ShrinkDc,
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
            // EVA
            EvaPop3070 = dto.EvaPop3070,
            EvaLdpe = dto.EvaLdpe,
            EvaDc = dto.EvaDc,
            EvaTalcol = dto.EvaTalcol,
            EvaSlip = dto.EvaSlip,
            EvaStaticAdditive = dto.EvaStaticAdditive,
            EvaOther = dto.EvaOther,
            // Other fields
            QuantityKg = dto.QuantityKg,
            RequiredDate = dto.RequiredDate,
            IsCompleted = dto.IsCompleted,
            Status = dto.Status,
            ActualCompletionDate = dto.ActualCompletionDate,
            DelayReason = dto.DelayReason
        };

        if (existingId.HasValue)
        {
            line.Id = existingId.Value;
        }

        return line;
    }

    private void UpdateLines(
        GrainMixingBlowingProcess grainMixingBlowingProcess,
        List<UpdateGrainMixingBlowingProcessLineDto> lineDtos)
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
            if (lineDto.Id.HasValue)
            {
                // Cập nhật line hiện có
                var existingLine = grainMixingBlowingProcess.Lines
                    .FirstOrDefault(l => l.Id == lineDto.Id.Value);

                if (existingLine != null)
                {
                    var updatedLine = MapUpdateToGrainMixingBlowingProcessLine(lineDto, lineDto.Id);
                    updatedLine.GrainMixingBlowingProcessId = existingLine.GrainMixingBlowingProcessId;
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapUpdateToGrainMixingBlowingProcessLine(lineDto);
                grainMixingBlowingProcess.Lines.Add(newLine);
            }
        }
    }

    #endregion
}
