using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;
using Fox.Whs.SapModels;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Production Orders từ SAP
/// </summary>
[ApiController]
[Route("api/production-orders")]
public class ProductionOrdersController : ControllerBase
{
    private readonly AppDbContext _sapDbContext;
    private readonly ILogger<ProductionOrdersController> _logger;

    public ProductionOrdersController(AppDbContext sapDbContext, ILogger<ProductionOrdersController> logger)
    {
        _sapDbContext = sapDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách Production Orders với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <returns>Danh sách Production Orders</returns>
    [HttpGet]
    public async Task<IActionResult> GetProductionOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }

        _logger.LogInformation("Lấy danh sách Production Orders - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var totalRecords = await _sapDbContext.ProductionOrders.AsNoTracking().CountAsync();

        var productionOrders = await _sapDbContext.ProductionOrders.AsNoTracking()
            .Include(po => po.ItemDetail)
            .Include(po => po.BusinessPartnerDetail)
            .OrderBy(po => po.DocEntry)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<ProductionOrder>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results = productionOrders
        });
    }
}
