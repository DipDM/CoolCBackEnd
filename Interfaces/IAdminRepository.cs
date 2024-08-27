using CoolCBackEnd.Models;
using System.Threading.Tasks;

namespace CoolCBackEnd.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> GetByUsernameAsync(string username);
        Task<Admin> RegisterAsync(Admin admin); // Ensure this returns Admin
        Task<bool> ValidatePasswordAsync(string username, string password);
    }
}
