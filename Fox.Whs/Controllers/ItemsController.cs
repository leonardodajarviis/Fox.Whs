using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;
using Fox.Whs.SapModels;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Items từ SAP
/// </summary>
[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _sapDbContext;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(AppDbContext sapDbContext, ILogger<ItemsController> logger)
    {
        _sapDbContext = sapDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách Items với phân trang
    /// </summary>
    /// <param name="search">keyword search</param>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <returns>Danh sách Items</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<Item>))]
    public async Task<IActionResult> GetItems([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

        var query = _sapDbContext.Items.AsNoTracking().AsQueryable();

        if (search is not null)
        {
            query = query.Where(i => i.ItemCode.Contains(search) || i.ItemName!.Contains(search));
        }

        var totalRecords = await query.CountAsync();

        var items = await query
            .OrderBy(i => i.ItemCode)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<Item>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results = items
        });
    }
}
