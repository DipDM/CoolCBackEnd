using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface ICouponUserRepository
    {
         Task<IEnumerable<CouponUser>> GetAllAsync();
    Task<CouponUser> GetByIdAsync(int CouponUserId);
    Task<CouponUser> CreateAsync(CouponUser couponUserModel);
    Task<CouponUser> UpdateAsync(int CouponUserId, CouponUser couponUserModel);
    Task<CouponUser> DeleteAsync(int CouponUserId);
    }
}