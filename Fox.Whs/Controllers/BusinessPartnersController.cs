using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.SapModels;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Business Partners từ SAP (Khách hàng)
/// </summary>
[ApiController]
[Route("api/business-partners")]
[Authorize]
public class BusinessPartnersController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public BusinessPartnersController(AppDbContext sapDbContext)
    {
        _dbContext = sapDbContext;
    }

    /// <summary>
    /// Lấy danh sách Business Partners với phân trang
    /// </summary>
    /// <param name="search"></param>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <returns>Danh sách Business Partners</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<BusinessPartner>))]
    public async Task<IActionResult> GetBusinessPartners([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }


        var query = _dbContext.BusinessPartners.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(bp => bp.CardCode.Contains(search) || bp.CardName!.Contains(search));
        }

        var totalRecords = await query.CountAsync();

        var businessPartners = await query
            .OrderBy(bp => bp.CardCode)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<BusinessPartner>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results = businessPartners
        });
    }
}
