using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CoolCBackEnd.Repository
{
    public class CouponOrderRepository : ICouponOrderRepository
    {
        private readonly ApplicationDBContext _context;

        public CouponOrderRepository(ApplicationDBContext context){
            _context = context;
        }
        public async Task<CouponOrder> CreatedAsync(CouponOrder couponorderModel)
        {
            await _context.CouponOrders.AddAsync(couponorderModel);
            await _context.SaveChangesAsync();
            return couponorderModel;
        }

        public async Task<CouponOrder> DeleteAsync(int CouponOrderId)
        {
            var couponorder = await _context.CouponOrders.FirstOrDefaultAsync( c => c.CouponOrderId == CouponOrderId);
            if(couponorder == null)
            {
                return null;
            }

            _context.CouponOrders.Remove(couponorder);
            await _context.SaveChangesAsync();
            return couponorder;
        }

        public async Task<IEnumerable<CouponOrder>> GetAllAsync()
        {
            return await _context.CouponOrders.ToListAsync();
        }

        public async Task<CouponOrder> GetByIdAsync(int CouponOrderId)
        {
            return await _context.CouponOrders.FirstOrDefaultAsync(c => c.CouponOrderId == CouponOrderId);
        }

        public async Task<CouponOrder> UpdatedAsync(int CouponOrderId, CouponOrder couponorderModel)
        {
            var existingCouponOrder = await _context.CouponOrders.FirstOrDefaultAsync( c=> c.CouponOrderId == CouponOrderId);
            if (existingCouponOrder == null)
            {
                return null;
            }

            existingCouponOrder.CouponId = couponorderModel.CouponId;
            existingCouponOrder.OrderId = couponorderModel.OrderId;
            existingCouponOrder.DiscountAmount = couponorderModel.DiscountAmount;

            await _context.SaveChangesAsync();
            return existingCouponOrder;
        }
    }
}