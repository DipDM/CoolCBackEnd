using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.CartItem;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem> CreateAsync(CartItem cartItem);
        Task<CartItem> DeleteAsync(int cartItemId);
        Task<List<CartItem>> GetAllAsync();
        Task<CartItem> GetByIdAsync(int cartItemId);
        Task<CartItem> UpdateAsync(int cartItemId, UpdateCartItemDto cartItemDto);
        Task<CartItem> AddOrUpdateCartItemAsync(int cartId, int productId, int quantity);
        

    }
}