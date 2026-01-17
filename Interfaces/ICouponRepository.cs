using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Coupon;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon> GetByIdAsync(int CouponId);
        Task<Coupon> CreatedAsync(Coupon couponModel);
        Task<Coupon> UpdatedAsync(int CouponId, Coupon couponModel);
        Task<Coupon> DeleteAsync(int CouponId); 
    }
}