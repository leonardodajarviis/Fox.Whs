using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Fox.Whs.Controllers;

/// <summary>
/// API quản lý danh sách Máy pha từ SAP
/// </summary>
[ApiController]
[Route("api/mixture-types")]
[Authorize]
public class MixtureTypeController : ControllerBase
{
    public MixtureTypeController()
    {
    }

    private static readonly string[] value =
        [
            "HD",
            "PE",
            "Màng co",
            "Màng chít",
            "PP"
        ];

    /// <summary>
    /// Lấy danh sách chủng loại pha
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(value); 
    }
}