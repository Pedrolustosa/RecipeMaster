using Microsoft.AspNetCore.Mvc;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Interfaces;
using RecipeMaster.Core.Exceptions;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        try
        {
            var result = await _authService.RegisterAsync(model);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(new { error = ex.Message, details = ex.Errors });
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while registering the user.", ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        try
        {
            var response = await _authService.LoginAsync(model);
            return Ok(response);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            throw new InternalServerException("An error occurred while logging in.", ex.Message);
        }
    }
}
