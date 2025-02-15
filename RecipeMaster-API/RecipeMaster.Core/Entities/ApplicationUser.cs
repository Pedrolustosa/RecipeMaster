using Microsoft.AspNetCore.Identity;

namespace RecipeMaster.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; } = true;
}
