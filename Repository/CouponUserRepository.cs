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
    public class CouponUserRepository : ICouponUserRepository
{
    private readonly ApplicationDBContext _context;

    public CouponUserRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<CouponUser> CreateAsync(CouponUser couponUserModel)
    {
        await _context.CouponUsers.AddAsync(couponUserModel);
        await _context.SaveChangesAsync();
        return couponUserModel;
    }

    public async Task<CouponUser> DeleteAsync(int CouponUserId)
    {
        var couponUser = await _context.CouponUsers.FirstOrDefaultAsync(c => c.CouponUserId == CouponUserId);
        if (couponUser == null)
        {
            return null;
        }

        _context.CouponUsers.Remove(couponUser);
        await _context.SaveChangesAsync();
        return couponUser;
    }

    public async Task<IEnumerable<CouponUser>> GetAllAsync()
    {
        return await _context.CouponUsers.Include(cu => cu.Coupon).ToListAsync();
    }

    public async Task<CouponUser> GetByIdAsync(int CouponUserId)
    {
        return await _context.CouponUsers.Include(cu => cu.Coupon)
                                         .FirstOrDefaultAsync(cu => cu.CouponUserId == CouponUserId);
    }

    public async Task<CouponUser> UpdateAsync(int CouponUserId, CouponUser couponUserModel)
    {
        var existingCouponUser = await _context.CouponUsers.FirstOrDefaultAsync(c => c.CouponUserId == CouponUserId);
        if (existingCouponUser == null)
        {
            return null;
        }

        existingCouponUser.UserId = couponUserModel.UserId;
        existingCouponUser.CouponId = couponUserModel.CouponId;
        existingCouponUser.RedeemedDate = couponUserModel.RedeemedDate;
        existingCouponUser.OrderId = couponUserModel.OrderId;

        await _context.SaveChangesAsync();
        return existingCouponUser;
    }
}

}