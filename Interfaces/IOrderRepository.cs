using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order orderModel);
        Task<Order> DeleteAsync(Guid OrderId);
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid OrderId);
        Task<Order> UpdateTotalAmountAsync(Guid orderId);
        Task<Order> CreateOrderFromCartAsync(int cartId, Guid userId);
    }
}