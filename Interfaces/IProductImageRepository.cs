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
        Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId);
        Task<ProductImage> UpdateAsync(int productImageId,IFormFile newImageFile);
        Task RemoveAsync(ProductImage productImage);

    }
}
