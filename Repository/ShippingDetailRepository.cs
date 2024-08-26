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
    public class ShippingDetailRepository : IShippingDetailRepository
{
    private readonly ApplicationDBContext _context;

    public ShippingDetailRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<ShippingDetail> CreateAsync(ShippingDetail shippingDetail)
    {
        _context.Set<ShippingDetail>().Add(shippingDetail);
        await _context.SaveChangesAsync();
        return shippingDetail;
    }

    public async Task<ShippingDetail> GetByIdAsync(int id)
    {
        return await _context.Set<ShippingDetail>().FindAsync(id);
    }

    public async Task<IEnumerable<ShippingDetail>> GetAllAsync()
    {
        return await _context.Set<ShippingDetail>().ToListAsync();
    }

    public async Task<ShippingDetail> UpdateAsync(int id, ShippingDetail shippingDetail)
    {
        var existingShippingDetail = await GetByIdAsync(id);
        if (existingShippingDetail == null) return null;

        _context.Entry(existingShippingDetail).CurrentValues.SetValues(shippingDetail);
        await _context.SaveChangesAsync();
        return existingShippingDetail;
    }

    public async Task DeleteAsync(int id)
    {
        var shippingDetail = await GetByIdAsync(id);
        if (shippingDetail != null)
        {
            _context.Set<ShippingDetail>().Remove(shippingDetail);
            await _context.SaveChangesAsync();
        }
    }
}

}