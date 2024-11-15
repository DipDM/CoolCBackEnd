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
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // Fetch ProductSize details
            var productSize = await _context.ProductSizes.FirstOrDefaultAsync(ps => ps.ProductSizeId == cartItem.SizeId);
            if (productSize == null)
            {
                throw new Exception("ProductSize not found.");
            }

            cartItem.Price = product.Price * cartItem.Quantity;
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
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
            cartItem.SizeId = cartItemDto.SizeId;

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



        public async Task<CartItem> AddOrUpdateCartItemAsync(int cartId, int productId, int quantity, int SizeId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var productSize = await _context.ProductSizes.FindAsync(SizeId);
            if (productSize == null)
            {
                throw new Exception("ProductSize not found.");
            }

            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId && ci.SizeId == SizeId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
                existingCartItem.Price = product.Price * existingCartItem.Quantity;
                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    SizeId = SizeId,
                    Price = product.Price * quantity
                };

                await _context.CartItems.AddAsync(newCartItem);
            }

            await _context.SaveChangesAsync();
            return existingCartItem ?? await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId && ci.SizeId == SizeId);
        }


    }
}