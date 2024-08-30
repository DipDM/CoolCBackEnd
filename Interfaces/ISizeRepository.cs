using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface ISizeRepository
    {
        Task<Size> CreateAsync(Size size);
        Task<Brand> UpdateAsync(int SizeId, Size size);
        Task<Brand> DeleteAsync(int SizeId);
        Task<List<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int SizeId);
    }
}