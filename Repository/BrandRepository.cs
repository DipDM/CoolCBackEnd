using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Brand;
using CoolCBackEnd.Dtos.Product;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDBContext _context;
        public BrandRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Brand> CreateAsync(Brand brandModel)
        {
            await _context.Brands.AddAsync(brandModel);
            await _context.SaveChangesAsync();
            return brandModel;
        }

        public async Task<Brand> DeleteAsync(int BrandId)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(c => c.BrandId == BrandId);
            if (brand == null)
            {
                return null;
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetByIdAsync(int BrandId)
        {
            return await _context.Brands.FirstOrDefaultAsync(c => c.BrandId == BrandId);
        }

        public async Task<List<BrandDto>> GetBrandsWithProductsAsync()
        {
            var brands = await _context.Brands
                .Include(b => b.Products)
                .ToListAsync();

            // Map Brand entity to BrandDto including the list of products
            var brandDtos = brands.Select(b => new BrandDto
            {
                BrandId = b.BrandId,
                Name = b.Name,
                NickName = b.NickName,
                Products = b.Products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    BrandId = p.BrandId,
                    CategoryId = p.CategoryId,
                    



                }).ToList() ?? new List<ProductDto>()
            }).ToList();

            return brandDtos;
        }


        public async Task<Brand> UpdateAsync(int BrandId, Brand brand)
        {
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(c => c.BrandId == BrandId);
            if (existingBrand == null)
            {
                return null;
            }

            existingBrand.Name = brand.Name;
            existingBrand.NickName = brand.NickName;

            await _context.SaveChangesAsync();
            return existingBrand;
        }
    }
}