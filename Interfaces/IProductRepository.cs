using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Product;
using CoolCBackEnd.Helpers;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(QueryObject query);
        Task<int> CountAsync(QueryObject query);
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreatedAsync(Product productModel);
        Task<Product> UpdatedAsync(int id, UpdateProductRequestDto productDto);
        Task<Product> DeleteAsync(int id);
        Task<bool> ProductExists(int id);
    }
}