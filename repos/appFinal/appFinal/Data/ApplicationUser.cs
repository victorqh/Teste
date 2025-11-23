using Microsoft.AspNetCore.Identity;

namespace appFinal.Data;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
