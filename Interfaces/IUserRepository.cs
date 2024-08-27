using CoolCBackEnd.Models;
using System.Threading.Tasks;

namespace CoolCBackEnd.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> RegisterAsync(User user); // Ensure this method returns a User
        Task<bool> ValidatePasswordAsync(string username, string password);
    }
}
