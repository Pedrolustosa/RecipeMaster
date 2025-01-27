using RecipeMaster.Core.Entities;

namespace RecipeMaster.Core.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest model);
    Task<TokenResponse> LoginAsync(LoginRequest model);
}
