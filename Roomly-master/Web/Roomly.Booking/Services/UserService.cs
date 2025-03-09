using System.Security.Claims;

namespace Roomly.Booking.Services;

public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Guid GetUserId()
    {
        var userIdClaim  = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        return Guid.Parse(userIdClaim);
    }
}