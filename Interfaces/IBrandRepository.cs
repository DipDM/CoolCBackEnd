using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Brand;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IBrandRepository
    {
        Task<Brand> CreateAsync(Brand brandModel);
        Task<Brand> UpdateAsync(int BrandId, Brand brand);
        Task<Brand> DeleteAsync(int BrandId);
        Task<List<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int BrandId);
        Task<List<BrandDto>> GetBrandsWithProductsAsync();
    }
}