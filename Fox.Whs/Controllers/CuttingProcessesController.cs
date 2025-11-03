using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fox.Whs.Dtos;
using Fox.Whs.Services;
using Fox.Whs.Models;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý công đoạn cắt
/// </summary>
[ApiController]
[Route("api/cutting-processes")]
public class CuttingProcessesController : ControllerBase
{
    private readonly CuttingProcessService _cuttingProcessService;

    public CuttingProcessesController(
        CuttingProcessService cuttingProcessService)
    {
        _cuttingProcessService = cuttingProcessService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn cắt
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<CuttingProcess>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        var cuttingProcesses = await _cuttingProcessService.GetAllAsync(qp);
        return Ok(cuttingProcesses);
    }

    /// <summary>
    /// Lấy theo công đoạn cắt theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CuttingProcess))]
    public async Task<IActionResult> GetById(int id)
    {
        var cuttingProcess = await _cuttingProcessService.GetByIdAsync(id);
        return Ok(cuttingProcess);
    }

    /// <summary>
    /// Tạo công đoạn cắt mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCuttingProcessDto dto)
    {
        var cuttingProcess = await _cuttingProcessService.CreateAsync(dto);
        return Ok(cuttingProcess);
    }

    /// <summary>
    /// Cập nhật công đoạn cắt
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCuttingProcessDto dto)
    {
        var cuttingProcess = await _cuttingProcessService.UpdateAsync(id, dto);
        return Ok(cuttingProcess);
    }

    /// <summary>
    /// Xóa công đoạn cắt
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cuttingProcessService.DeleteAsync(id);
        return NoContent();
    }
}
