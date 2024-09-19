using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Category;
using CoolCBackEnd.Dtos.Product;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;

        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category categoryModel)
        {
            await _context.Categories.AddAsync(categoryModel);
            await _context.SaveChangesAsync();
            return categoryModel;
        }

        public async Task<Category> UpdateAsync(int CategoryId, Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == CategoryId);
            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<Category> DeleteAsync(int CategoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == CategoryId);
            if (category == null)
            {
                return null;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int CategoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == CategoryId);
        }

        public async Task<List<CategoryDto>> GetCategoriesWithProductsAsync()
        {
            var categories = await _context.Categories
                .Include(b => b.Products)
                .ToListAsync();

            var categoryDtos = categories.Select( c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                Products = c.Products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    BrandId = p.BrandId,
                    CategoryId = p.CategoryId
                }).ToList() ?? new List<ProductDto>()
            }).ToList();

            return categoryDtos;
        }
    }
}