using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface ICouponOrderRepository
    {
        Task<IEnumerable<CouponOrder>> GetAllAsync();
        Task<CouponOrder> GetByIdAsync(int CouponOrderId);
        Task<CouponOrder> CreatedAsync(CouponOrder couponorderModel);
        Task<CouponOrder> UpdatedAsync(int CouponOrderId, CouponOrder couponorderModel);
        Task<CouponOrder> DeleteAsync(int CouponOrderId);
    }
}