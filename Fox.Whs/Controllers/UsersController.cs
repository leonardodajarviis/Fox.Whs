using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.Exceptions;
using Fox.Whs.SapModels;
using Fox.Whs.Dtos;
using Gridify;
using Microsoft.AspNetCore.Authorization;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Users từ SAP (Người dùng)
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public UsersController(AppDbContext sapDbContext)
    {
        _dbContext = sapDbContext;
    }

    /// <summary>
    /// Lấy danh sách Users với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <param name="search"></param>
    /// <returns>Danh sách Users</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<User>))]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }


        var query = _dbContext.Users.AsNoTracking().AsQueryable();

        if (search is not null) query = query.Where(x => search.Contains(x.FullName) || search.Contains(x.Username));
        var totalRecords              = await query.CountAsync();


        var users = await query
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<User>
        {
            Page       = page,
            PageSize   = pageSize,
            TotalCount = totalRecords,
            Results    = users
        });
    }
}