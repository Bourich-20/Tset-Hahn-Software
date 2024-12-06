using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ExpenseTrackerAPI.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

[HttpPost("register")]
[AllowAnonymous]
public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
{
    if (registerRequest == null)
        return BadRequest(new { Message = "User object cannot be null." });

    try
    {
        var token = await _userService.RegisterAsync(registerRequest);

        return Ok(new { Message = "User registered successfully", Token = token });
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new { Message = ex.Message });
    }
}


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null)
            return BadRequest(new { Message = "User object cannot be null." });

        try
        {
            var token = await _userService.LoginAsync(loginRequest);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }
}

}
