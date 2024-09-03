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
        Task<Size> UpdateAsync(int SizeId, Size size);
        Task<Size> DeleteAsync(int SizeId);
        Task<List<Size>> GetAllAsync();
        Task<Size> GetByIdAsync(int SizeId);
    }
}