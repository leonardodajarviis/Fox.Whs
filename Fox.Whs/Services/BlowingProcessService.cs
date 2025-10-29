using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

/// <summary>
/// Service quản lý công đoạn thổi
/// </summary>
public class BlowingProcessService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<BlowingProcessService> _logger;

    public BlowingProcessService(
        AppDbContext dbContext,
        ILogger<BlowingProcessService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn thổi
    /// </summary>
    public async Task<PaginationResponse<BlowingProcess>> GetAllAsync(QueryParam pr)
    {
        var query = _dbContext.BlowingProcesses.AsNoTracking().ApplyFiltering(pr).AsQueryable();


        var totalCount = await query.CountAsync();

        var result = await query
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
        var blowingProcess = await _dbContext.BlowingProcesses
            .Include(bp => bp.ShiftLeader)
            .Include(bp => bp.Lines)
                .ThenInclude(line => line.Employee)
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
        _logger.LogInformation("Tạo công đoạn thổi mới cho trưởng ca {LeaderId}", dto.ShiftLeaderId);

        // Kiểm tra trưởng ca tồn tại
        var shiftLeaderExists = await _dbContext.Employees
            .AnyAsync(e => e.Id == dto.ShiftLeaderId);

        if (!shiftLeaderExists)
        {
            throw new NotFoundException($"Không tìm thấy trưởng ca với ID: {dto.ShiftLeaderId}");
        }

        // Kiểm tra công nhân thổi có tồn tại không
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

            var missingWorkers = workerIds.Except(existingWorkers).ToList();
            if (missingWorkers.Any())
            {
                throw new NotFoundException($"Không tìm thấy công nhân với ID: {string.Join(", ", missingWorkers)}");
            }
        }

        var blowingProcess = new BlowingProcess
        {
            ShiftLeaderId = dto.ShiftLeaderId,
            ProductionDate = dto.ProductionDate,
            ProductionShift = dto.ProductionShift,
            Lines = [.. dto.Lines.Select(MapToBlowingProcessLine)]
        };

        // Tính toán tổng
        CalculateTotals(blowingProcess);

        _dbContext.BlowingProcesses.Add(blowingProcess);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Đã tạo công đoạn thổi với ID: {Id}", blowingProcess.Id);

        return await GetByIdAsync(blowingProcess.Id);
    }

    /// <summary>
    /// Cập nhật công đoạn thổi
    /// </summary>
    public async Task<BlowingProcess> UpdateAsync(int id, UpdateBlowingProcessDto dto)
    {
        _logger.LogInformation("Cập nhật công đoạn thổi với ID: {Id}", id);

        var blowingProcess = await _dbContext.BlowingProcesses
            .Include(bp => bp.Lines)
            .FirstOrDefaultAsync(bp => bp.Id == id);

        if (blowingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn thổi với ID: {id}");
        }

        // Kiểm tra trưởng ca tồn tại
        var shiftLeaderExists = await _dbContext.Employees
            .AnyAsync(e => e.Id == dto.ShiftLeaderId);

        if (!shiftLeaderExists)
        {
            throw new NotFoundException($"Không tìm thấy trưởng ca với ID: {dto.ShiftLeaderId}");
        }

        // Kiểm tra công nhân thổi có tồn tại không
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

            var missingWorkers = workerIds.Except(existingWorkers).ToList();
            if (missingWorkers.Any())
            {
                throw new NotFoundException($"Không tìm thấy công nhân với ID: {string.Join(", ", missingWorkers)}");
            }
        }

        // Cập nhật thông tin cơ bản
        blowingProcess.ShiftLeaderId = dto.ShiftLeaderId;
        blowingProcess.ProductionDate = dto.ProductionDate;
        blowingProcess.ProductionShift = dto.ProductionShift;

        // Cập nhật lines
        UpdateLines(blowingProcess, dto.Lines);

        // Tính toán lại tổng
        CalculateTotals(blowingProcess);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Đã cập nhật công đoạn thổi với ID: {Id}", id);

        return await GetByIdAsync(id);
    }

    /// <summary>
    /// Xóa công đoạn thổi
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Xóa công đoạn thổi với ID: {Id}", id);

        var blowingProcess = await _dbContext.BlowingProcesses
            .Include(bp => bp.Lines)
            .FirstOrDefaultAsync(bp => bp.Id == id);

        if (blowingProcess == null)
        {
            throw new NotFoundException($"Không tìm thấy công đoạn thổi với ID: {id}");
        }

        _dbContext.BlowingProcesses.Remove(blowingProcess);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Đã xóa công đoạn thổi với ID: {Id}", id);
    }

    #region Private Methods

    private BlowingProcessLine MapToBlowingProcessLine(CreateBlowingProcessLineDto dto)
    {
        var line = new BlowingProcessLine
        {
            ItemCode = dto.ItemCode,
            ProductionBatch = dto.ProductionBatch,
            CustomerName = dto.CustomerName,
            ProductType = dto.ProductType,
            Thickness = dto.Thickness,
            SemiProductWidth = dto.SemiProductWidth,
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
            WeighingDate = dto.WeighingDate,
            IsCompleted = dto.IsCompleted,
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

    private BlowingProcessLine MapToBlowingProcessLine(UpdateBlowingProcessLineDto dto, int? existingId = null)
    {
        var line = new BlowingProcessLine
        {
            ItemCode = dto.ItemCode,
            ProductionBatch = dto.ProductionBatch,
            CustomerName = dto.CustomerName,
            ProductType = dto.ProductType,
            Thickness = dto.Thickness,
            SemiProductWidth = dto.SemiProductWidth,
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
            WeighingDate = dto.WeighingDate,
            IsCompleted = dto.IsCompleted,
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

    private void UpdateLines(BlowingProcess blowingProcess, List<UpdateBlowingProcessLineDto> lineDtos)
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
            if (lineDto.Id.HasValue)
            {
                // Cập nhật line hiện có
                var existingLine = blowingProcess.Lines.FirstOrDefault(l => l.Id == lineDto.Id.Value);
                if (existingLine != null)
                {
                    var updatedLine = MapToBlowingProcessLine(lineDto, lineDto.Id.Value);
                    _dbContext.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                }
            }
            else
            {
                // Thêm line mới
                var newLine = MapToBlowingProcessLine(lineDto);
                blowingProcess.Lines.Add(newLine);
            }
        }
    }

    private void CalculateTotals(BlowingProcess blowingProcess)
    {
        blowingProcess.TotalBlowingOutput = blowingProcess.Lines.Sum(l => l.QuantityKg);
        blowingProcess.TotalRewindingOutput = blowingProcess.Lines.Sum(l => l.RewindOrSplitWeight);
        blowingProcess.TotalReservedOutput = blowingProcess.Lines.Sum(l => l.ReservedWeight);
        blowingProcess.TotalBlowingLoss = blowingProcess.Lines.Sum(l => l.TotalLoss);
    }

    #endregion
}
