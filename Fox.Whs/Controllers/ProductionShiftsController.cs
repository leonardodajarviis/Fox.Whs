using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;
using Fox.Whs.SapModels;
using Microsoft.AspNetCore.Authorization;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Ca sản xuất từ SAP
/// </summary>
[ApiController]
[Route("api/production-shifts")]
[Authorize]
public class ProductionShiftsController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProductionShiftsController> _logger;

    public ProductionShiftsController(AppDbContext sapDbContext, ILogger<ProductionShiftsController> logger)
    {
        _dbContext = sapDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách ProductionShifts với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <returns>Danh sách Items</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductionShift>))]
    public async Task<IActionResult> GetProductionShifts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }

        _logger.LogInformation("Lấy danh sách Items - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var totalRecords = await _dbContext.Items.AsNoTracking().CountAsync();

        var items = await _dbContext.ProductionShifts.AsNoTracking()
            .OrderBy(i => i.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<ProductionShift>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results = items
        });
    }
}
