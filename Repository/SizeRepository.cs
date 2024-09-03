using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class SizeRepository : ISizeRepository
    {
        private readonly ApplicationDBContext _context;

        public SizeRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Size> CreateAsync(Size size)
        {
            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();
            return size;
        }

        public async Task<Size> DeleteAsync(int SizeId)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(c => c.SizeId == SizeId);
            if(size == null)
            {
                return null;
            }
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            return size;
        }

        public async Task<List<Size>> GetAllAsync()
        {
            return await _context.Sizes.ToListAsync();
        }

        public async Task<Size> GetByIdAsync(int SizeId)
        {
            return await _context.Sizes.FirstOrDefaultAsync( c => c.SizeId == SizeId);
        }

        public async Task<Size> UpdateAsync(int SizeId, Size size)
        {
            var existingSize = await _context.Sizes.FirstOrDefaultAsync(c => c.SizeId == SizeId);
            if (existingSize == null)
            {
                return null;
            }


            existingSize.SizeName = size.SizeName;
            await _context.SaveChangesAsync();
            return existingSize;
        }
    }
}