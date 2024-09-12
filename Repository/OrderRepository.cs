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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;

        public OrderRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateAsync(Order orderModel)
        {
            await _context.Orders.AddAsync(orderModel);
            await _context.SaveChangesAsync();
            return orderModel;
        }

        public async Task<Order> DeleteAsync(Guid OrderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(c => c.OrderId == OrderId);
            if (order == null)
            {
                return null;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order> GetByIdAsync(Guid OrderId)
        {
            return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(c => c.OrderId == OrderId);
        }

        public async Task<Order> UpdateTotalAmountAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return null;
            }

            order.TotalAmount = order.OrderItems.Sum(oi => oi.Price);
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }


        // Method to create an order from cart items
        public async Task<Order> CreateOrderFromCartAsync(int cartId, Guid userId)
        {
            // Fetch the cart items
            var cartItems = await _context.CartItems
                                          .Where(ci => ci.CartId == cartId)
                                          .ToListAsync();

            // Calculate total amount
            var totalAmount = cartItems.Sum(ci => ci.Quantity * ci.Price);

            // Create new order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderStatus = "Pending", // initial status
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList()
            };

            // Save the order and order items
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Optional: Clear cart after order is created
            await ClearCartAsync(cartId);

            return order;
        }

        // Optional method to clear the cart
        private async Task ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                                          .Where(ci => ci.CartId == cartId)
                                          .ToListAsync();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

    }
}