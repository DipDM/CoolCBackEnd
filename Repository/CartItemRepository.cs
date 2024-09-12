using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.CartItem;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ICartRepository _cartRepository;

        public CartItemRepository(ApplicationDBContext context, ICartRepository cartRepository)
        {
            _context = context;
            _cartRepository = cartRepository;
        }


        public async Task<CartItem> CreateAsync(CartItem cartItem)
        {
            // Fetch the product to get the price
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }


            Console.WriteLine($"Fetched Product Price: {product.Price}");

            // Ensure the price is set to the product price multiplied by quantity
            cartItem.Price = product.Price * cartItem.Quantity;

            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            // Recalculate the total cart amount
            await _cartRepository.UpdateCartTotalAmountAsync(cartItem.CartId);

            return cartItem;
        }

        public async Task<CartItem> DeleteAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return null;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<List<CartItem>> GetAllAsync()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem> GetByIdAsync(int cartItemId)
        {
            return await _context.CartItems.FindAsync(cartItemId);
        }

        public async Task<CartItem> UpdateAsync(int cartItemId, UpdateCartItemDto cartItemDto)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return null;

            cartItem.Quantity = cartItemDto.Quantity;

            // Fetch the product again to ensure we have the latest price
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // Recalculate price based on updated quantity and product price
            cartItem.Price = product.Price * cartItem.Quantity;

            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            // Recalculate the total cart amount
            await _cartRepository.UpdateCartTotalAmountAsync(cartItem.CartId);

            return cartItem;
        }


        public async Task<CartItem> AddOrUpdateCartItemAsync(int cartId, int productId, int quantity)
        {
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (existingCartItem != null)
            {
                // Update quantity and price
                existingCartItem.Quantity += quantity;

                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    existingCartItem.Price = product.Price * existingCartItem.Quantity;
                }

                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                // Add new cart item
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return null; // or handle product not found
                }

                var newCartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price * quantity
                };

                await _context.CartItems.AddAsync(newCartItem);
            }

            await _context.SaveChangesAsync();
            return existingCartItem ?? await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

    }
}