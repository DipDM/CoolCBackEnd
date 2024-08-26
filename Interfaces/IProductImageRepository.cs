using System.Collections.Generic;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IProductImageRepository
    {
        Task<ProductImage> CreateAsync(IFormFile imageFile, int productId);
        Task<ProductImage> DeleteAsync(int id);
        Task<List<ProductImage>> GetAllAsync();
        Task<ProductImage> GetByIdAsync(int id);
        Task<ProductImage> UpdatedAsync(int id, ProductImage productImageModel);
        Task RemoveAsync(ProductImage productImage);

    }
}
