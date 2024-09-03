using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Product;
using CoolCBackEnd.Helpers;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;

        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<bool> ProductExists(int productId)
        {
            return await _context.Products.AnyAsync(s => s.ProductId == productId);
        }

        public async Task<int> CountAsync(QueryObject query)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(s => s.Name.Contains(query.Name));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                products = products.Where(s => s.Description.Contains(query.Description));
            }
            return await products.CountAsync();
        }

        public async Task<Product> CreatedAsync(Product productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (productModel == null)
            {
                return null;
            }
            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<List<Product>> GetAllAsync(QueryObject query)
        {
            var products = _context.Products
            .Include(p => p.ProductImages)
            .Include(c => c.ProductSizes)
            .ThenInclude(l => l.Size)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(s => s.Name.Contains(query.Name));
            }
            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                products = products.Where(s => s.Description.Contains(query.Description));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                switch (query.SortBy.Trim().ToLower())
                {
                    case "Name":
                        products = query.IsDescending
                            ? products.OrderByDescending(s => s.Name)
                            : products.OrderBy(s => s.Name);
                        break;

                    case "Description":
                        products = query.IsDescending
                            ? products.OrderByDescending(s => s.Description)
                            : products.OrderBy(s => s.Description);
                        break;

                        // Add additional cases here for other sorting options if needed
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await products.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.ProductImages).Include( f => f.ProductSizes).ThenInclude(ps => ps.Size)
                                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> UpdatedAsync(int id, UpdateProductRequestDto productDto)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;

            await _context.SaveChangesAsync();
            return existingProduct;
        }
    }
}