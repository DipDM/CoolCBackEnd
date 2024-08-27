using CoolCBackEnd.Dtos.Admin;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IConfiguration _configuration;

    public AdminService(IAdminRepository adminRepository, IConfiguration configuration)
    {
        _adminRepository = adminRepository;
        _configuration = configuration;
    }

    public async Task<AuthResult> RegisterAsync(AdminRegisterDto registerDto)
    {
        var existingAdmin = await _adminRepository.GetByUsernameAsync(registerDto.Username);
        if (existingAdmin != null)
        {
            return new AuthResult
            {
                Success = false,
                Error = "Username already exists."
            };
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var admin = new Admin
        {
            Username = registerDto.Username,
            PasswordHash = passwordHash,
            Role = "Admin" // Set a default role if needed
        };

        var registeredAdmin = await _adminRepository.RegisterAsync(admin);

        var token = JwtTokenHolder.GenerateToken(registeredAdmin.Username, registeredAdmin.Role,
            _configuration["Jwt:Key"], _configuration["Jwt:Issuer"]);

        return new AuthResult
        {
            Token = token,
            Success = true
        };
    }

    public async Task<AuthResult> LoginAsync(AdminLoginDto loginDto)
    {
        var admin = await _adminRepository.GetByUsernameAsync(loginDto.Username);
        if (admin == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.PasswordHash))
        {
            return new AuthResult
            {
                Success = false,
                Error = "Invalid username or password"
            };
        }

        var token = JwtTokenHolder.GenerateToken(admin.Username, admin.Role,
            _configuration["Jwt:Key"], _configuration["Jwt:Issuer"]);

        return new AuthResult
        {
            Token = token,
            Success = true
        };
    }

    public async Task<AuthResult> AuthenticateAsync(string username, string password)
    {
        var admin = await _adminRepository.GetByUsernameAsync(username);
        if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
        {
            return new AuthResult
            {
                Success = false,
                Error = "Invalid username or password"
            };
        }

        var token = JwtTokenHolder.GenerateToken(admin.Username, admin.Role,
            _configuration["Jwt:Key"], _configuration["Jwt:Issuer"]);

        return new AuthResult
        {
            Admin = admin,
            Token = token,
            Success = true
        };
    }
}
