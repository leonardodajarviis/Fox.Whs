using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.SapModels;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Lệnh sản xuất pha hạt từ SAP
/// </summary>
[ApiController]
[Route("api/production-order-grain-mixings")]
[Authorize]
public class ProductionOrderGrainMixingsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ProductionOrderGrainMixingsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Lấy danh sách Lệnh sản xuất pha hạt với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <param name="cardCode"></param>
    /// <returns>Danh sách Lệnh sản xuất pha hạt</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductionOrderGrainMixing>))]
    public async Task<IActionResult> GetProductionOrderGrainMixings([FromQuery] string? cardCode, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }

        var query = _dbContext.ProductionOrderGrainMixings.AsNoTracking().Where(u => u.Status != "Y");

        if (!string.IsNullOrEmpty(cardCode))
        {
            query = query.Where(u => u.CardCode == cardCode);
        }

        var totalRecords = await query.CountAsync();

        var productionOrders = await query
            .OrderByDescending(p => p.DocEntry)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<ProductionOrderGrainMixing>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results = productionOrders
        });
    }
}
