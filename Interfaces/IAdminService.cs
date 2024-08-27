using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Admin;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IAdminService
    {
        Task<AuthResult> RegisterAsync(AdminRegisterDto registerDto);
        Task<AuthResult> LoginAsync(AdminLoginDto loginDto);
        Task<AuthResult> AuthenticateAsync(string username, string password);
    }
}