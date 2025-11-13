using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.SapModels;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Máy chia từ SAP
/// </summary>
[ApiController]
[Route("api/slitting-machines")]
[Authorize]
public class SlittingMachinesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public SlittingMachinesController(AppDbContext sapDbContext)
    {
        _dbContext = sapDbContext;
    }

    /// <summary>
    /// Lấy danh sách Máy tua với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <returns>Danh sách Máy tua</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<Printer>))]
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


        var totalRecords = await _dbContext.SlittingMachines.AsNoTracking().CountAsync();

        var slittingMachines = await _dbContext.SlittingMachines.AsNoTracking()
            .OrderBy(b => b.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<SlittingMachine>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results = slittingMachines
        });
    }
}
