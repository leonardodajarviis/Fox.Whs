using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;
using Fox.Whs.SapModels;
using Microsoft.AspNetCore.Authorization;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Production Orders từ SAP (Lệch sản xuất)
/// </summary>
[ApiController]
[Route("api/production-orders")]
[Authorize]
public class ProductionOrdersController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ProductionOrdersController(AppDbContext sapDbContext)
    {
        _dbContext = sapDbContext;
    }

    /// <summary>
    /// Lấy danh sách Production Orders với phân trang
    /// </summary>
    /// <param name="type">Loại công đoạn</param>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <param name="itemCode">Lọc theo mã hàng hóa</param>
    /// <returns>Danh sách Production Orders</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductionOrder>))]
    public async Task<IActionResult> GetProductionOrders(
        [FromQuery] string? type,
        [FromQuery] string? itemCode,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }


        var query = _dbContext.ProductionOrders.AsNoTracking().AsQueryable();

        if (itemCode is not null)
        {
            query = query.Where(po => po.ItemCode == itemCode);
        }

        query = ApplyFilterProductionOrderType(query, type);

        var totalRecords = await query.CountAsync();

        var productionOrders = await query
            .Include(po => po.ItemDetail)
            .ThenInclude(po => po!.ProductTypeInfo)
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

    private static IQueryable<ProductionOrder> ApplyFilterProductionOrderType(
        IQueryable<ProductionOrder> query,
        string? type
    )
    {
        if (string.IsNullOrEmpty(type)) return query;

        switch (type)
        {
            case "printing":
                query = query
                    .Where(po => (po.PrintingStatus == "N" || po.PrintingStatus == null) && po.IsPrinting == "Y");
                break;
            case "blowing":
                query = query
                    .Where(po => (po.BlowingStatus == "N" || po.BlowingStatus == null) && po.IsBlowing == "Y");
                break;
            default:
                break;
        }

        return query;
    }
}
