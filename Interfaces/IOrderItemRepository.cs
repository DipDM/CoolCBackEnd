using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.OrderItem;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IOrderItemRepository
    {
         Task<OrderItem> CreateAsync(OrderItem OrderItem);
        Task<OrderItem> DeleteAsync(int OrderItemId);
        Task<List<OrderItem>> GetAllAsync();
        Task<OrderItem> GetByIdAsync(int OrderItemId);
        Task<OrderItem> UpdateAsync(int OrderItemId, UpdateOrderItemRequestDto OrderItemDto);
        Task<List<CartItem>> GetCartItemsAsync(int cartId);
        Task<List<OrderItem>> CreateOrderItemsFromCartAsync(Guid orderId, List<CartItem> cartItems);
    }
}