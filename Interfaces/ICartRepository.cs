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
        Task<Cart> GetCartByUserIdAsync(Guid userId);
        Task<Cart> UpdateAsync(int cartId,Cart updatedCart);
        Task<Cart> UpdateCartTotalAmountAsync(int CartId);
        Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId);
    }
}