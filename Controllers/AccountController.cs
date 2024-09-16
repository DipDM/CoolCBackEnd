using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.User;
using CoolCBackEnd.Helpers;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using CoolCBackEnd.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger<AccountController> _logger;
        private readonly IOtpCacheService _otpCacheService;
        private readonly SignInManager<User> _signingManager;
        public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, IEmailService emailService, IOtpCacheService otpCacheService, ILogger<AccountController> logger, LinkGenerator linkGenerator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signingManager = signInManager;
            _emailService = emailService;
            _otpCacheService = otpCacheService;
            _linkGenerator = linkGenerator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userloginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Convert email to lowercase
            var email = userloginDto.Email.ToLower();

            // Find the user by email instead of UserName
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return Unauthorized("Invalid email!");
            }

            // Check password
            var result = await _signingManager.CheckPasswordSignInAsync(user, userloginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Password incorrect");
            }

            // Fetch user roles
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    UserId = user.Id,
                    Token = _tokenService.CreateToken(user, roles.ToList()), // Pass roles here
                    roles = roles.ToList() // Include roles in the response
                }
            );
        }


        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the username or email already exists
                var existingUserByUsername = await _userManager.FindByNameAsync(userRegisterDto.Username);
                if (existingUserByUsername != null)
                {
                    return BadRequest("Username already exists.");
                }

                var existingUserByEmail = await _userManager.FindByEmailAsync(userRegisterDto.Email);
                if (existingUserByEmail != null)
                {
                    return BadRequest("Email address already exists.");
                }

                // Create the user
                var appUser = new User
                {
                    UserName = userRegisterDto.Username,
                    Email = userRegisterDto.Email,
                    CreationTime = DateTime.UtcNow
                };

                var createdUser = await _userManager.CreateAsync(appUser, userRegisterDto.Password);

                if (createdUser.Succeeded)
                {
                    // Assign role
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (roleResult.Succeeded)
                    {
                        // Generate OTP and Verification Link
                        var otp = OtpHelper.GenerateOtp();  // Method to generate OTP
                        var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                        var verificationLink = Url.Action("ConfirmEmail", "Auth", new { userId = appUser.Id, token = verificationToken }, protocol: Request.Scheme);

                        _logger.LogInformation("Generated token: {Token}", verificationToken);
                        _logger.LogInformation("Request scheme: {Scheme}", Request?.Scheme);
                        _logger.LogInformation("Generated verification link: {Link}", verificationLink);

                        // Send email with OTP and Verification Link
                        var emailBody = $@"
                                            <p>Thank you for registering. Please verify your email by either entering the OTP below or clicking the verification link:</p>
            <p><strong>OTP: {otp}</strong></p>
            <p>this<a href='{verificationLink}'>Click here to verify your email</a></p>";

                        await _emailService.SendEmailAsync(appUser.Email, "Email Verification", emailBody);  // Using the EmailService to send email
                        _logger.LogInformation("Verification email sent successfully to {Email}", appUser.Email);

                        // Store OTP in database or cache (for this example, we use a hypothetical CacheService)
                        _otpCacheService.StoreOtp(appUser.Email, otp, TimeSpan.FromMinutes(30));

                        // Fetch roles for the newly registered user
                        var roles = await _userManager.GetRolesAsync(appUser);

                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                UserId = appUser.Id,
                                Token = _tokenService.CreateToken(appUser, roles.ToList()), // Pass roles here
                                roles = roles.ToList(), // Include roles in the response
                                Message = "User registered successfully. Please check your email for verification."
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
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




        [HttpPost("test")]
        public IActionResult TestUrlGeneration()
        {
            var testLink = Url.Action("ConfirmEmail", "Auth", new { userId = "testUserId", token = "testToken" }, protocol: Request.Scheme);
            _logger.LogInformation("Test URL: {Url}", testLink);
            return Ok(testLink);
        }


        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail()
        {
            await _emailService.SendEmailAsync("dipeshmendhe@gmail.com", "http://exampbhbkhle.com/verify", "123456");
            return Ok("Email sent");
        }



        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationDto otpDto)
        {
            try
            {
                // Retrieve the user by email
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == otpDto.Email);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Verify OTP logic
                var storedOtp = _otpCacheService.GetOtp(user.Email);
                if (storedOtp == null || storedOtp != otpDto.Otp)
                {
                    return BadRequest("Invalid OTP.");
                }

                // Complete the email confirmation or next steps
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                return Ok("Email verified successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully.");
            }

            return BadRequest("Error confirming email.");
        }



        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the username or email already exists
                var existingUserByUsername = await _userManager.FindByNameAsync(userRegisterDto.Username);
                if (existingUserByUsername != null)
                {
                    return BadRequest("Username already exists.");
                }

                var existingUserByEmail = await _userManager.FindByEmailAsync(userRegisterDto.Email);
                if (existingUserByEmail != null)
                {
                    return BadRequest("Email address already exists.");
                }

                // Create the user
                var appUser = new User
                {
                    UserName = userRegisterDto.Username,
                    Email = userRegisterDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, userRegisterDto.Password);

                if (createdUser.Succeeded)
                {
                    // Assign the "Admin" role to this user
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Admin");

                    if (roleResult.Succeeded)
                    {
                        // Generate OTP and Verification Link
                        var otp = OtpHelper.GenerateOtp();  // Method to generate OTP
                        var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                        var verificationLink = Url.Action("ConfirmEmail", "Auth", new { userId = appUser.Id, token = verificationToken }, protocol: Request.Scheme);

                        _logger.LogInformation("Generated token: {Token}", verificationToken);
                        _logger.LogInformation("Request scheme: {Scheme}", Request?.Scheme);
                        _logger.LogInformation("Generated verification link: {Link}", verificationLink);

                        // Send email with OTP and Verification Link
                        var emailBody = $@"
                                    <p>Thank you for registering as an admin. Please verify your email by either entering the OTP below or clicking the verification link:</p>
                <p><strong>OTP: {otp}</strong></p>
                <p>or <a href='{verificationLink}'>Click here to verify your email</a></p>";

                        await _emailService.SendEmailAsync(appUser.Email, "Email Verification", emailBody);  // Using the EmailService to send email
                        _logger.LogInformation("Verification email sent successfully to {Email}", appUser.Email);

                        // Store OTP in database or cache (for this example, we use a hypothetical CacheService)
                        _otpCacheService.StoreOtp(appUser.Email, otp, TimeSpan.FromMinutes(30));

                        // Fetch roles for the newly registered admin
                        var roles = await _userManager.GetRolesAsync(appUser);

                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                UserId = appUser.Id,
                                Token = _tokenService.CreateToken(appUser, roles.ToList()), // Pass roles here
                                roles = roles.ToList(), // Include roles in the response
                                Message = "Admin registered successfully. Please check your email for verification."
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
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

        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return Ok("User deleted successfully.");
                }
                else
                {
                    return StatusCode(500, result.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
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


        [HttpPost("send-reset-otp")]
        public async Task<IActionResult> SendResetOtp([FromBody] SendResetOtpRequestDto requestDto)
        {
            try
            {
                if (requestDto == null || string.IsNullOrWhiteSpace(requestDto.Email))
                {
                    return BadRequest("Email is required.");
                }

                var user = await _userManager.FindByEmailAsync(requestDto.Email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Generate OTP
                var otp = OtpHelper.GenerateOtp();  // Implement this method to generate OTP

                // Store OTP in cache or database (for verification later)
                _otpCacheService.StoreOtp(requestDto.Email, otp, TimeSpan.FromMinutes(10));  // 10 minutes validity

                // Send the OTP via email
                await _emailService.SendEmailAsync(user.Email, "Password Reset Request", $"Use this OTP to reset your password: {otp}");

                return Ok("OTP sent to email.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                // Verify OTP
                var isOtpValid = _otpCacheService.ValidateOtp(resetPasswordDto.Email, resetPasswordDto.Otp);
                if (!isOtpValid)
                {
                    return BadRequest("Invalid OTP.");
                }

                // Find user
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Generate token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Unable to generate password reset token.");
                }

                // Reset the password
                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);

                if (resetPasswordResult.Succeeded)
                {
                    return Ok("Password reset successfully.");
                }

                return BadRequest(resetPasswordResult.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error resetting password");
                return StatusCode(500, e.Message);
            }
        }



        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                if (_userManager == null)
                {
                    _logger.LogError("UserManager service is not initialized.");
                    return StatusCode(500, "Internal server error.");
                }

                // Find user by UserId
                var user = await _userManager.FindByIdAsync(changePasswordDto.UserId.ToString());
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Check current password
                var passwordCheckResult = await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
                if (!passwordCheckResult)
                {
                    return BadRequest("Current password is incorrect.");
                }

                // Change password
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    return Ok("Password changed successfully.");
                }

                var errorMessages = changePasswordResult.Errors.Select(e => e.Description).ToList();
                return BadRequest(errorMessages);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error changing password");
                return StatusCode(500, "An error occurred while changing the password.");
            }
        }


        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                if (_userManager == null)
                {
                    _logger.LogError("UserManager service is not initialized.");
                    return StatusCode(500, "Internal server error.");
                }

                // Find user by UserId
                var user = await _userManager.FindByIdAsync(updateUserDto.UserId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Update username if provided
                if (!string.IsNullOrEmpty(updateUserDto.NewUsername))
                {
                    var usernameExists = await _userManager.FindByNameAsync(updateUserDto.NewUsername);
                    if (usernameExists != null && usernameExists.Id != user.Id)
                    {
                        return BadRequest("Username is already taken.");
                    }

                    user.UserName = updateUserDto.NewUsername;
                }

                // Update phone number if provided
                if (!string.IsNullOrEmpty(updateUserDto.NewPhoneNumber))
                {
                    user.PhoneNumber = updateUserDto.NewPhoneNumber;
                }

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return Ok("User details updated successfully.");
                }

                var errorMessages = updateResult.Errors.Select(e => e.Description).ToList();
                return BadRequest(errorMessages);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating user details");
                return StatusCode(500, "An error occurred while updating the user details.");
            }
        }








    }


}