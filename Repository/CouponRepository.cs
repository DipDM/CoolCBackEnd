using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Coupon;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CoolCBackEnd.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDBContext _context;

        public CouponRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Coupon> CreatedAsync(Coupon couponModel)
        {
            await _context.Coupons.AddAsync(couponModel);
            await _context.SaveChangesAsync();
            return couponModel;
        }

        public async Task<Coupon> DeleteAsync(int CouponId)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync( c => c.CouponId == CouponId );
            if(coupon == null)
            {
                return null;
            }

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task<Coupon> GetByIdAsync(int CouponId)
        {
            return await _context.Coupons.FirstOrDefaultAsync(c => c.CouponId == CouponId);
        }

        public async Task<Coupon> UpdatedAsync(int CouponId, Coupon couponModel)
        {
            var existingCoupon = await _context.Coupons.FirstOrDefaultAsync( c => c.CouponId == CouponId);
            if(existingCoupon == null)
            {
                return null;
            }

            existingCoupon.Code = couponModel.Code;
            existingCoupon.DiscountType = couponModel.DiscountType;
            existingCoupon.DiscountValue = couponModel.DiscountValue;
            existingCoupon.MaxDiscountAmount = couponModel.MaxDiscountAmount;
            existingCoupon.MinOrderAmount = couponModel.MinOrderAmount;
            existingCoupon.StartDate = couponModel.StartDate;
            existingCoupon.EndDate = couponModel.EndDate;
            existingCoupon.IsActive = couponModel.IsActive;
            existingCoupon.UsageLimit = couponModel.UsageLimit;
            existingCoupon.UsageCount = couponModel.UsageCount;

            await _context.SaveChangesAsync();
            return existingCoupon;
        }
    }
}