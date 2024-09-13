using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.User;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Controllers
{
    public class AccountController : ControllerBase
    {
        public readonly UserManager<User> _userManager;
        public readonly ITokenService _tokenService;
        public readonly SignInManager<User> _signingManager;
        public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signingManager = signInManager;

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userloginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userloginDto.UserName.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid Username!");
            }

            var result = await _signingManager.CheckPasswordSignInAsync(user, userloginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("User not found/Password incorrect");
            }

            // Fetch user roles
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user, roles.ToList()), // Pass roles here
                    roles = roles.ToList() // Include roles in the response
                }
            );
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userregisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new User
                {
                    UserName = userregisterDto.Username,
                    Email = userregisterDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, userregisterDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (roleResult.Succeeded)
                    {
                        // Fetch roles for the newly registered user
                        var roles = await _userManager.GetRolesAsync(appUser);

                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser, roles.ToList()), // Pass roles here
                                roles = roles.ToList() // Include roles in the response
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, createdUser.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = new User
            {
                UserName = userRegisterDto.Username,
                Email = userRegisterDto.Email
            };

            var result = await _userManager.CreateAsync(appUser, userRegisterDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign the "Admin" role to this user
            var roleResult = await _userManager.AddToRoleAsync(appUser, "Admin");
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            // Fetch roles for the newly registered admin
            var roles = await _userManager.GetRolesAsync(appUser);

            return Ok(new NewUserDto
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser, roles.ToList()), // Pass roles here
                roles = roles.ToList() // Include roles in the response
            });
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]  // Only accessible by admins
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignDto roleAssignDto)
        {
            var user = await _userManager.FindByIdAsync(roleAssignDto.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, roleAssignDto.Role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Role assigned successfully.");
        }

        [HttpPost("remove-role")]
        [Authorize(Roles = "Admin")]  // Only accessible by admins
        public async Task<IActionResult> RemoveRole([FromBody] RoleAssignDto roleAssignDto)
        {
            var user = await _userManager.FindByIdAsync(roleAssignDto.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleAssignDto.Role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Role removed successfully.");
        }


    }


}