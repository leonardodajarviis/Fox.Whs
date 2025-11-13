using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fox.Whs.Dtos;
using Fox.Whs.Services;
using Fox.Whs.Models;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý công đoạn pha hạt (Thổi)
/// </summary>
[ApiController]
[Route("api/grain-mixing-blowing-processes")]
[Authorize]
public class GrainMixingBlowingProcessesController : ControllerBase
{
    private readonly GrainMixingBlowingProcessService _grainMixingBlowingProcessService;

    public GrainMixingBlowingProcessesController(
        GrainMixingBlowingProcessService grainMixingBlowingProcessService)
    {
        _grainMixingBlowingProcessService = grainMixingBlowingProcessService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn pha hạt (Thổi)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<GrainMixingBlowingProcess>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        var grainMixingBlowingProcesses = await _grainMixingBlowingProcessService.GetAllAsync(qp);
        return Ok(grainMixingBlowingProcesses);
    }

    /// <summary>
    /// Lấy công đoạn pha hạt (Thổi) theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrainMixingBlowingProcess))]
    public async Task<IActionResult> GetById(int id)
    {
        var grainMixingBlowingProcess = await _grainMixingBlowingProcessService.GetByIdAsync(id);
        return Ok(grainMixingBlowingProcess);
    }

    /// <summary>
    /// Tạo công đoạn pha hạt (Thổi) mới
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrainMixingBlowingProcess))]
    public async Task<IActionResult> Create([FromBody] CreateGrainMixingBlowingProcessDto dto)
    {
        var grainMixingBlowingProcess = await _grainMixingBlowingProcessService.CreateAsync(dto);
        return Ok(grainMixingBlowingProcess);
    }

    /// <summary>
    /// Cập nhật công đoạn pha hạt (Thổi)
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrainMixingBlowingProcess))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGrainMixingBlowingProcessDto dto)
    {
        var grainMixingBlowingProcess = await _grainMixingBlowingProcessService.UpdateAsync(id, dto);
        return Ok(grainMixingBlowingProcess);
    }

    /// <summary>
    /// Xóa công đoạn pha hạt (Thổi)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await _grainMixingBlowingProcessService.DeleteAsync(id);
        return NoContent();
    }
}
