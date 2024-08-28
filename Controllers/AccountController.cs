using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.User;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
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
        public AccountController(UserManager<User> userManager,ITokenService tokenService,SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signingManager = signInManager;

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userloginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userloginDto.UserName.ToLower());

            if(user == null)
            {
                return Unauthorized("Invalid USername!");

            }

            var result = await _signingManager.CheckPasswordSignInAsync(user, userloginDto.Password,false);

            if(!result.Succeeded)
            {
                return Unauthorized("User not found/Password incorrect");
            }

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userregisterDto)
        {
            try{
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new User{
                    UserName = userregisterDto.Username,
                    Email = userregisterDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser,userregisterDto.Password);

                if(createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser,"User");

                    if(roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
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
                    return StatusCode(500,createdUser.Errors);
                }
            }catch(Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}