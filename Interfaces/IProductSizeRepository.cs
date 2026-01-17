using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IProductSizeRepository
    {
        Task<ProductSize> CreateAsync (ProductSize productSize);
        Task<ProductSize> UpdateAsync(int ProductSizeId, ProductSize productSize);
        Task<ProductSize> DeleteAsync(int ProductSizeId);
        Task<List<ProductSize>> GetAllAsync();
        Task<ProductSize> GetByIdAsync(int ProductSizeId);
    }
}