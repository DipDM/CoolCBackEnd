
using CoolCBackEnd.Dtos.User;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;
    private readonly SignInManager<User> _signingManager;

    public UserController(UserManager<User> userManager, IUserService userService, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _userService = userService;
        _signingManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
    {
        try
        {
            // Check if the ModelState is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new User object
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            // Create the user
            var createUserResult = await _userManager.CreateAsync(user, registerDto.Password);

            // Check if user creation succeeded
            if (createUserResult.Succeeded)
            {
                // Assign the user to the "User" role
                var roleResult = await _userManager.AddToRoleAsync(user, "User");

                if (roleResult.Succeeded)
                {
                    // Return success response with token
                    return Ok(new NewUserDto
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = _userService.CreateUserToken(user)
                    });
                }
                else
                {
                    // Return bad request with role assignment errors
                    return BadRequest(new
                    {
                        Errors = roleResult.Errors.Select(e => e.Description)
                    });
                }
            }
            else
            {
                // Return bad request with user creation errors
                return BadRequest(new
                {
                    Errors = createUserResult.Errors.Select(e => e.Description)
                });
            }
        }
        catch (Exception e)
        {
            // Log the exception and return a server error response
            // Consider logging e.Message or e.ToString() for debugging
            return StatusCode(500, new
            {
                Message = "An error occurred while processing your request.",
                Details = e.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        if (user == null)
        {
            return Unauthorized("Invalid USername!");

        }

        var result = await _signingManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized("User not found/Password incorrect");
        }

        return Ok(
            new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _userService.CreateUserToken(user)
            }
        );
    }
}

