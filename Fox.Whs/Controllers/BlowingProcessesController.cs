using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fox.Whs.Dtos;
using Fox.Whs.Services;
using Fox.Whs.Models;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý công đoạn thổi
/// </summary>
[ApiController]
[Route("api/blowing-processes")]
[Authorize]
public class BlowingProcessesController : ControllerBase
{
    private readonly BlowingProcessService _blowingProcessService;

    public BlowingProcessesController(
        BlowingProcessService blowingProcessService)
    {
        _blowingProcessService = blowingProcessService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn thổi
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<BlowingProcess>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        var blowingProcesses = await _blowingProcessService
            .GetAllAsync(qp);
        return Ok(blowingProcesses);
    }


    /// <summary>
    /// Lấy theo công đoạn thổi theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlowingProcess))]
    public async Task<IActionResult> GetById(int id)
    {
        var blowingProcesses = await _blowingProcessService.GetByIdAsync(id);
        return Ok(blowingProcesses);
    }

    /// <summary>
    /// Tạo công đoạn thổi mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBlowingProcessDto dto)
    {
        var blowingProcess = await _blowingProcessService.CreateAsync(dto);
        return Ok(blowingProcess);
    }

    /// <summary>
    /// Cập nhật công đoạn thổi
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBlowingProcessDto dto)
    {
        var blowingProcess = await _blowingProcessService.UpdateAsync(id, dto);
        return Ok(blowingProcess);
    }

    /// <summary>
    /// Xóa công đoạn thổi
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _blowingProcessService.DeleteAsync(id);
        return NoContent();
    }
}
