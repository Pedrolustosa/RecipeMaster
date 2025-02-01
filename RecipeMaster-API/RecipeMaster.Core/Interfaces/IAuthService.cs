using RecipeMaster.Core.Entities;

namespace RecipeMaster.Core.Interfaces.Repositories;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest model);
    Task<TokenResponse> LoginAsync(LoginRequest model);
    Task<ApplicationUser> GetUserByIdAsync(string id);
    Task<ApplicationUser> GetUserByEmailAsync(string email);
    Task<ApplicationUser> GetUserByUsernameAsync(string username);
    Task<bool> UpdateUserAsync(UpdateUserRequest model);
    Task<bool> DeactivateUserAsync(string id);
}
