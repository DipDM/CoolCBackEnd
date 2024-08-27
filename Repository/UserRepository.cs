using CoolCBackEnd.Data;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CoolCBackEnd.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == UserName);
        }

        public async Task<User> RegisterAsync(User user)
        {
            _context.Users.Add(user); // Adding the User entity
            await _context.SaveChangesAsync(); // Save changes to the database
            return user; // Return the User entity
        }

        public async Task<bool> ValidatePasswordAsync(string username, string password)
        {
            var user = await GetByUsernameAsync(username);
            if (user == null)
            {
                return false;
            }

            // Validate the hashed password
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }

}
