using Fox.Whs.Data;
using Fox.Whs.Dtos;
using Fox.Whs.Exceptions;
using Fox.Whs.Models;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Services;

public class AuditLogService
{
    private readonly AppDbContext _dbContext;

    public AuditLogService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginationResponse<AuditLog>> GetAllAsync(QueryParam queryParam)
    {
        var query = _dbContext.AuditLogs
            .AsNoTracking()
            .ApplyFiltering(queryParam)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var result = await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .ApplyOrderingAndPaging(queryParam)
            .ToListAsync();

        return new PaginationResponse<AuditLog>
        {
            Results = result,
            TotalCount = totalCount,
            Page = queryParam.Page,
            PageSize = queryParam.PageSize
        };
    }

    public async Task<AuditLog> GetByIdAsync(long id)
    {
        var log = await _dbContext.AuditLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (log == null)
        {
            throw new NotFoundException($"Không tìm thấy audit log với ID: {id}");
        }

        return log;
    }
}
