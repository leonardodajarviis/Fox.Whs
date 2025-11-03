using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Fox.Whs.Dtos;
using Fox.Whs.Services;
using Fox.Whs.Models;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý công đoạn in
/// </summary>
[ApiController]
[Route("api/printing-processes")]
[Authorize]
public class PrintingProcessesController : ControllerBase
{
    private readonly PrintingProcessService _printingProcessService;

    public PrintingProcessesController(
        PrintingProcessService printingProcessService)
    {
        _printingProcessService = printingProcessService;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn in
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<PrintingProcess>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        var printingProcesses = await _printingProcessService.GetAllAsync(qp);
        return Ok(printingProcesses);
    }

    /// <summary>
    /// Lấy theo công đoạn in theo ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrintingProcess))]
    public async Task<IActionResult> GetById(int id)
    {
        var printingProcess = await _printingProcessService.GetByIdAsync(id);
        return Ok(printingProcess);
    }

    /// <summary>
    /// Tạo công đoạn in mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePrintingProcessDto dto)
    {
        var printingProcess = await _printingProcessService.CreateAsync(dto);
        return Ok(printingProcess);
    }

    /// <summary>
    /// Cập nhật công đoạn in
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePrintingProcessDto dto)
    {
        var printingProcess = await _printingProcessService.UpdateAsync(id, dto);
        return Ok(printingProcess);
    }

    /// <summary>
    /// Xóa công đoạn in
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _printingProcessService.DeleteAsync(id);
        return NoContent();
    }
}
