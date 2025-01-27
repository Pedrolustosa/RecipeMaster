using System.Text;
using RecipeMaster.Core.JWT;
using System.Security.Claims;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Exceptions;
using RecipeMaster.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace RecipeMaster.Infra.Identity.Services;

public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtSettings jwtSettings) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<string> RegisterAsync(RegisterRequest model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.ToDictionary(
                e => e.Code,
                e => new[] { e.Description }));
        }

        return "User registered successfully";
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded) throw new UnauthorizedException("Invalid credentials");

        var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new NotFoundException("User", model.Email);
        var token = GenerateJwtToken(user);

        return new TokenResponse { Token = token };
    }

    public string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
