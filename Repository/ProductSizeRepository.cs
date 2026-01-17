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
    public class ProductSizeRepository : IProductSizeRepository
    {
        private readonly ApplicationDBContext _context;

        public ProductSizeRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<ProductSize> CreateAsync(ProductSize productSize)
        {
            await _context.ProductSizes.AddAsync(productSize);
            await _context.SaveChangesAsync();
            return productSize;
        }

        public async Task<ProductSize> DeleteAsync(int ProductSizeId)
        {
            var productSize = await _context.ProductSizes.FirstOrDefaultAsync(x => x.ProductSizeId == ProductSizeId);
            if(productSize == null)
            {
                return null;
            }
            _context.ProductSizes.Remove(productSize);
            await _context.SaveChangesAsync();
            return productSize;
        }

        public async Task<List<ProductSize>> GetAllAsync()
        {
            return await _context.ProductSizes.ToListAsync();
        }

        public async Task<ProductSize> GetByIdAsync(int ProductSizeId)
        {
            return await _context.ProductSizes.FirstOrDefaultAsync(c => c.ProductSizeId == ProductSizeId);
        }

        public async Task<ProductSize> UpdateAsync(int ProductSizeId, ProductSize productSize)
        {
            var existingProductSize = await _context.ProductSizes.FirstOrDefaultAsync(c => c.ProductSizeId == ProductSizeId);
            if(existingProductSize == null)
            {
                return null;
            }
            existingProductSize.SizeId = productSize.SizeId;
            await _context.SaveChangesAsync();
            return existingProductSize;
        }
    }
}