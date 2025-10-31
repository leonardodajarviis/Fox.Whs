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
public class PrintingProcessesController : ControllerBase
{
    private readonly PrintingProcessService _printingProcessService;
    private readonly ILogger<PrintingProcessesController> _logger;

    public PrintingProcessesController(
        PrintingProcessService printingProcessService,
        ILogger<PrintingProcessesController> logger)
    {
        _printingProcessService = printingProcessService;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách tất cả công đoạn in
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<PrintingProcess>))]
    public async Task<IActionResult> GetAll([FromQuery] QueryParam qp)
    {
        _logger.LogInformation("Lấy danh sách tất cả công đoạn in");
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
        _logger.LogInformation("Lấy công đoạn in theo ID: {Id}", id);
        var printingProcess = await _printingProcessService.GetByIdAsync(id);
        return Ok(printingProcess);
    }

    /// <summary>
    /// Tạo công đoạn in mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePrintingProcessDto dto)
    {
        _logger.LogInformation("Tạo công đoạn in mới");
        var printingProcess = await _printingProcessService.CreateAsync(dto);
        return Ok(printingProcess);
    }

    /// <summary>
    /// Cập nhật công đoạn in
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePrintingProcessDto dto)
    {
        _logger.LogInformation("Cập nhật công đoạn in với ID: {Id}", id);
        var printingProcess = await _printingProcessService.UpdateAsync(id, dto);
        return Ok(printingProcess);
    }

    /// <summary>
    /// Xóa công đoạn in
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Xóa công đoạn in với ID: {Id}", id);
        await _printingProcessService.DeleteAsync(id);
        return NoContent();
    }
}
