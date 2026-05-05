using Fox.Whs.Dtos;
using Fox.Whs.Models;
using Fox.Whs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fox.Whs.Controllers;

/// <summary>
/// API xem lịch sử tác động dữ liệu
/// </summary>
[ApiController]
[Route("api/audit-logs")]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly AuditLogService _auditLogService;

    public AuditLogsController(AuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Lấy danh sách audit logs (hỗ trợ filter/sort/paging bằng QueryParam)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<AuditLog>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        var logs = await _auditLogService.GetAllAsync(qp);
        return Ok(logs);
    }

    /// <summary>
    /// Lấy chi tiết audit log theo ID
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuditLog))]
    public async Task<IActionResult> GetById(long id)
    {
        var log = await _auditLogService.GetByIdAsync(id);
        return Ok(log);
    }
}
