using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fox.Whs.Dtos;
using Fox.Whs.Services;
using Fox.Whs.Models;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý công đoạn chia
/// </summary>
[ApiController]
[Route("api/slitting-processes")]
[Authorize]
public class SlittingProcessesController : ControllerBase
{
    private readonly SlittingProcessService _slittingProcessService;

    public SlittingProcessesController(
        SlittingProcessService slittingProcessService)
    {
        _slittingProcessService = slittingProcessService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn chia
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<SlittingProcess>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        var slittingProcesses = await _slittingProcessService
            .GetAllAsync(qp);
        return Ok(slittingProcesses);
    }

    /// <summary>
    /// Lấy công đoạn chia theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlittingProcess))]
    public async Task<IActionResult> GetById(int id)
    {
        var slittingProcess = await _slittingProcessService.GetByIdAsync(id);
        return Ok(slittingProcess);
    }

    /// <summary>
    /// Tạo công đoạn chia mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSlittingProcessDto dto)
    {
        var slittingProcess = await _slittingProcessService.CreateAsync(dto);
        return Ok(slittingProcess);
    }

    /// <summary>
    /// Cập nhật công đoạn chia
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSlittingProcessDto dto)
    {
        var slittingProcess = await _slittingProcessService.UpdateAsync(id, dto);
        return Ok(slittingProcess);
    }

    /// <summary>
    /// Xóa công đoạn chia
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _slittingProcessService.DeleteAsync(id);
        return NoContent();
    }
}
