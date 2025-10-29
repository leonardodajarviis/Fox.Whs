using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.Exceptions;
using Fox.Whs.SapModels;
using Fox.Whs.Dtos;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Users từ SAP
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _sapDbContext;
    private readonly ILogger<UsersController> _logger;

    public UsersController(AppDbContext sapDbContext, ILogger<UsersController> logger)
    {
        _sapDbContext = sapDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách Users với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <returns>Danh sách Users</returns>
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }

        _logger.LogInformation("Lấy danh sách Users - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var totalRecords = await _sapDbContext.Users.AsNoTracking().CountAsync();

        var users = await _sapDbContext.Users.AsNoTracking()
            .Include(u => u.GroupAssignments)
            .ThenInclude(ga => ga.Group)
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<User>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results = users
        });
    }
}
