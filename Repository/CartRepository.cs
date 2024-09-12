using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _context;

        public CartRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Cart> CreateAsync(Cart cartModel)
        {
            await _context.Carts.AddAsync(cartModel);
            await _context.SaveChangesAsync();
            return cartModel;
        }

        public async Task<Cart> DeleteAsync(int CartId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == CartId);
            if (cart == null)
            {
                return null;
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<List<Cart>> GetAllAsync()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems) // If you want to include cart items
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart> GetByIdAsync(int CartId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.CartId == CartId);
        }

        public async Task<Cart> UpdateAsync(int cartId, Cart updatedCart)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null) return null;

            cart.TotalAmount = updatedCart.TotalAmount;
            // Update other fields as needed

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> UpdateCartTotalAmountAsync(int cartId)
        {
            // Fetch the cart with all related cart items
            var cart = await _context.Carts
                .Include(c => c.CartItems)  // Include related CartItems
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart == null) return null;

            // Calculate the total amount by summing the prices of all cart items
            cart.TotalAmount = cart.CartItems.Sum(ci => ci.Price);

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return cart;
        }
        public async Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }


    }
}