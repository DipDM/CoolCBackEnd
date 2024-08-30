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

        public async Task<Brand> DeleteAsync(int SizeId)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(c => c.SizeId == SIzeId)
        }

        public Task<List<Brand>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Brand> GetByIdAsync(int SizeId)
        {
            throw new NotImplementedException();
        }

        public Task<Brand> UpdateAsync(int SizeId, Size size)
        {
            throw new NotImplementedException();
        }
    }
}