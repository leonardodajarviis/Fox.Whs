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

    public EmployeesController(AppDbContext sapDbContext)
    {
        _dbContext = sapDbContext;
    }

    /// <summary>
    /// Lấy danh sách Employees với phân trang
    /// </summary>
    /// <param name="page">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Số bản ghi trên mỗi trang (mặc định: 10)</param>
    /// <param name="search"></param>
    /// <returns>Danh sách Items</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<Employee>))]
    public async Task<IActionResult> GetEmployees([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
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


        var query = _dbContext.Employees.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => x.LastName!.Contains(search) || x.FirstName!.Contains(search));
        }


        var totalRecords = await query.CountAsync();

        var employees = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PaginationResponse<Employee>
        {
            Page       = page,
            PageSize   = pageSize,
            TotalCount = totalRecords,
            Results    = employees
        });
    }

    /// <summary>
    /// Lấy Employees theo Id
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>Danh sách Items</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
    public async Task<IActionResult> GetEmployeeById([FromQuery] int id)
    {

        var employees = await _dbContext.Employees.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

        if (employees == null)
        {
            throw new NotFoundException($"Không tìm thấy nhân viên với id = {id}");
        }

        return Ok(employees);
    }
}