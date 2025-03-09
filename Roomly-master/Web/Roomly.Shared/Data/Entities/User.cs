using Microsoft.AspNetCore.Identity;

namespace Roomly.Shared.Data.Entities;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly CreatedAt { get; set; }
}