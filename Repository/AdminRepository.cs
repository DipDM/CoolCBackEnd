using CoolCBackEnd.Data;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CoolCBackEnd.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDBContext _context;

        public AdminRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetByUsernameAsync(string username)
        {
            return await _context.Set<Admin>().FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<Admin> RegisterAsync(Admin admin)
        {
            _context.Set<Admin>().Add(admin);
            await _context.SaveChangesAsync();
            return admin; // Ensure this returns the Admin object
        }

        public async Task<bool> ValidatePasswordAsync(string username, string password)
        {
            var admin = await GetByUsernameAsync(username);
            if (admin == null)
            {
                return false;
            }

            // Validate the hashed password
            return BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash);
        }
    }

}
