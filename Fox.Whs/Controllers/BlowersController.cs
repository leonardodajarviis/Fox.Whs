using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.SapModels;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Máy thổi từ SAP
/// </summary>
[ApiController]
[Route("api/blowers")]
public class BlowersController : ControllerBase
{
    private readonly AppDbContext _sapDbContext;
    private readonly ILogger<BlowersController> _logger;

    public BlowersController(AppDbContext sapDbContext, ILogger<BlowersController> logger)
    {
        _sapDbContext = sapDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách Máy thổi với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <returns>Danh sách Máy thổi</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<Blower>))]
    public async Task<IActionResult> GetBlowers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }

        _logger.LogInformation("Lấy danh sách Máy thổi - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var totalRecords = await _sapDbContext.Blowers.AsNoTracking().CountAsync();

        var blowers = await _sapDbContext.Blowers.AsNoTracking()
            .OrderBy(b => b.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<Blower>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results = blowers
        });
    }
}
