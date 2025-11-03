using System.Security.Claims;

namespace Fox.Whs.Services;

public class UserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public short? GetCurrentUserId()
    {
        var userIdClaim = GetCurrentUser()?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return short.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    public string? GetCurrentUsername()
    {
        return GetCurrentUser()?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public int? GetCurrentEmployeeId()
    {
        var employeeIdClaim = GetCurrentUser()?.FindFirst("employeeId")?.Value;
        if (employeeIdClaim == "no-employee")
            return null;
        return int.TryParse(employeeIdClaim, out var employeeId) ? employeeId : null;
    }

    public ClaimsPrincipal? GetCurrentUser()
    {
        return _httpContextAccessor.HttpContext?.User;
    }

    public bool IsAuthenticated()
    {
        return GetCurrentUser()?.Identity?.IsAuthenticated == true;
    }
}
