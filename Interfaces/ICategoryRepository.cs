using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Category;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category categoryModel);
        Task<Category> UpdateAsync(int CategoryId, Category category);
        Task<Category> DeleteAsync(int CategoryId);
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int CategoryId);
        Task<List<CategoryDto>> GetCategoriesWithProductsAsync();
    }
}