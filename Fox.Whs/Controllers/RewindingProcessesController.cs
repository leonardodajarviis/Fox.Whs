using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fox.Whs.Dtos;
using Fox.Whs.Services;
using Fox.Whs.Models;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý công đoạn tua
/// </summary>
[ApiController]
[Route("api/rewinding-processes")]
[Authorize]
public class RewindingProcessesController : ControllerBase
{
    private readonly RewindingProcessService _rewindingProcessService;

    public RewindingProcessesController(
        RewindingProcessService rewindingProcessService)
    {
        _rewindingProcessService = rewindingProcessService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn tua
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<RewindingProcess>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        var rewindingProcesses = await _rewindingProcessService
            .GetAllAsync(qp);
        return Ok(rewindingProcesses);
    }

    /// <summary>
    /// Lấy công đoạn tua theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RewindingProcess))]
    public async Task<IActionResult> GetById(int id)
    {
        var rewindingProcess = await _rewindingProcessService.GetByIdAsync(id);
        return Ok(rewindingProcess);
    }

    /// <summary>
    /// Tạo công đoạn tua mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRewindingProcessDto dto)
    {
        var rewindingProcess = await _rewindingProcessService.CreateAsync(dto);
        return Ok(rewindingProcess);
    }

    /// <summary>
    /// Cập nhật công đoạn tua
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRewindingProcessDto dto)
    {
        var rewindingProcess = await _rewindingProcessService.UpdateAsync(id, dto);
        return Ok(rewindingProcess);
    }

    /// <summary>
    /// Xóa công đoạn tua
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _rewindingProcessService.DeleteAsync(id);
        return NoContent();
    }
}
