
using CoolCBackEnd.Dtos.Admin;
using CoolCBackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AdminRegisterDto registerDto)
    {
        var result = await _adminService.RegisterAsync(registerDto);
        if (result.Success)
        {
            return Ok(new { Token = result.Token });
        }
        return BadRequest(result.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginDto loginDto)
    {
        var result = await _adminService.LoginAsync(loginDto);
        if (result.Success)
        {
            return Ok(new { Token = result.Token });
        }
        return Unauthorized(result.Message);
    }
}

