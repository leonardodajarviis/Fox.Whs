using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fox.Whs.Data;
using Fox.Whs.Exceptions;
using Fox.Whs.Dtos;
using Fox.Whs.SapModels;
using Microsoft.AspNetCore.Authorization;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Employees từ SAP (Nhân viên)
/// </summary>
[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(AppDbContext sapDbContext, ILogger<EmployeesController> logger)
    {
        _dbContext = sapDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách Employees với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <returns>Danh sách Items</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<Employee>))]
    public async Task<IActionResult> GetEmployees([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page phải lớn hơn 0");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new BadRequestException("PageSize phải từ 1 đến 100");
        }

        _logger.LogInformation("Lấy danh sách Employees - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var totalRecords = await _dbContext.Employees.AsNoTracking().CountAsync();

        var employees = await _dbContext.Employees.AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<Employee>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalRecords,
            Results =  employees
        });
    }
}

