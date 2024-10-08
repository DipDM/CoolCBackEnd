using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.OrderItem;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
         private readonly ApplicationDBContext _context;

        public OrderItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> CreateAsync(OrderItem OrderItem)
        {
            await _context.OrderItems.AddAsync(OrderItem);
            await _context.SaveChangesAsync();
            return OrderItem;
        }

        public async Task<OrderItem> DeleteAsync(int OrderItemId)
        {
            var OrderItem = await _context.OrderItems.FindAsync(OrderItemId);
            if (OrderItem == null) return null;

            _context.OrderItems.Remove(OrderItem);
            await _context.SaveChangesAsync();
            return OrderItem;
        }

        public async Task<List<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem> GetByIdAsync(int OrderItemId)
        {
            return await _context.OrderItems.FindAsync(OrderItemId);
        }

        public async Task<OrderItem> UpdateAsync(int OrderItemId, UpdateOrderItemRequestDto OrderItemDto)
        {
            var OrderItem = await _context.OrderItems.FindAsync(OrderItemId);
            if (OrderItem == null) return null;

            OrderItem.Quantity = OrderItemDto.Quantity;

            _context.OrderItems.Update(OrderItem);
            await _context.SaveChangesAsync();
            return OrderItem;
        }
    }
}