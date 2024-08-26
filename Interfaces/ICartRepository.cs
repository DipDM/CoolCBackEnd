using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> CreateAsync(Cart cartModel);
        Task<Cart> DeleteAsync(int CartId);
        Task<List<Cart>> GetAllAsync();
        Task<Cart> GetByIdAsync(int CartId);
    }
}