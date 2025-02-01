using Microsoft.AspNetCore.Mvc;
using RecipeMaster.Core.Entities;
using RecipeMaster.Core.Exceptions;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

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

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        _logger.LogInformation("Searching for user by ID {Id}...", id);
        try
        {
            var user = await _authService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "User with ID {Id} not found.", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching for user by ID {Id}.", id);
            throw new InternalServerException("An error occurred while searching for the user.", ex.Message);
        }
    }

    [HttpGet("user/email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        _logger.LogInformation("Searching for user by email {Email}...", email);
        try
        {
            var user = await _authService.GetUserByEmailAsync(email);
            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "User with email {Email} not found.", email);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching for user by email {Email}.", email);
            throw new InternalServerException("An error occurred while searching for the user.", ex.Message);
        }
    }

    [HttpGet("user/username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        _logger.LogInformation("Searching for user by username {Username}...", username);
        try
        {
            var user = await _authService.GetUserByUsernameAsync(username);
            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "User with username {Username} not found.", username);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching for user by username {Username}.", username);
            throw new InternalServerException("An error occurred while searching for the user.", ex.Message);
        }
    }

    [HttpPut("user/{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest model)
    {
        _logger.LogInformation("Updating user with ID {Id}...", id);
        try
        {
            model.Id = id;
            var result = await _authService.UpdateUserAsync(model);
            if (result)
            {
                _logger.LogInformation("User with ID {Id} updated successfully.", id);
                return NoContent();
            }
            else
            {
                _logger.LogWarning("User with ID {Id} update did not succeed.", id);
                return BadRequest(new { error = "User update failed" });
            }
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "User with ID {Id} not found for update.", id);
            return NotFound(new { error = ex.Message });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error while updating user with ID {Id}.", id);
            return UnprocessableEntity(new { error = ex.Message, details = ex.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID {Id}.", id);
            throw new InternalServerException("An error occurred while updating the user.", ex.Message);
        }
    }

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeactivateUser(string id)
    {
        _logger.LogInformation("Deactivating user with ID {Id}...", id);
        try
        {
            var result = await _authService.DeactivateUserAsync(id);
            if (result)
            {
                _logger.LogInformation("User with ID {Id} deactivated successfully.", id);
                return NoContent();
            }
            else
            {
                _logger.LogWarning("User with ID {Id} deactivation did not succeed.", id);
                return BadRequest(new { error = "User deactivation failed" });
            }
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "User with ID {Id} not found for deactivation.", id);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deactivating user with ID {Id}.", id);
            throw new InternalServerException("An error occurred while deactivating the user.", ex.Message);
        }
    }
}
