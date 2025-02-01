using Microsoft.AspNetCore.Identity;

namespace RecipeMaster.Core.Entities;

public class ApplicationUser : IdentityUser
{
    // Additional properties can be added here if needed, e.g., FirstName, LastName
    public bool IsActive { get; set; } = true;
}
