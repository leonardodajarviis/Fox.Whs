using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fox.Whs.Dtos;
using Fox.Whs.Services;
using Fox.Whs.Models;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý công đoạn pha hạt
/// </summary>
[ApiController]
[Route("api/grain-mixing-processes")]
[Authorize]
public class GrainMixingProcessesController : ControllerBase
{
    private readonly GrainMixingProcessService _grainMixingProcessService;

    public GrainMixingProcessesController(
        GrainMixingProcessService grainMixingProcessService)
    {
        _grainMixingProcessService = grainMixingProcessService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn pha hạt
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<GrainMixingProcess>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        var grainMixingProcesses = await _grainMixingProcessService.GetAllAsync(qp);
        return Ok(grainMixingProcesses);
    }

    /// <summary>
    /// Lấy công đoạn pha hạt theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrainMixingProcess))]
    public async Task<IActionResult> GetById(int id)
    {
        var grainMixingProcess = await _grainMixingProcessService.GetByIdAsync(id);
        return Ok(grainMixingProcess);
    }

    /// <summary>
    /// Tạo công đoạn pha hạt mới
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrainMixingProcess))]
    public async Task<IActionResult> Create([FromBody] CreateGrainMixingProcessDto dto)
    {
        var grainMixingProcess = await _grainMixingProcessService.CreateAsync(dto);
        return Ok(grainMixingProcess);
    }

    /// <summary>
    /// Cập nhật công đoạn pha hạt
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrainMixingProcess))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGrainMixingProcessDto dto)
    {
        var grainMixingProcess = await _grainMixingProcessService.UpdateAsync(id, dto);
        return Ok(grainMixingProcess);
    }

    /// <summary>
    /// Xóa công đoạn pha hạt
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await _grainMixingProcessService.DeleteAsync(id);
        return NoContent();
    }
}
