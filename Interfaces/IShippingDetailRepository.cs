using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IShippingDetailRepository
    {
        Task<ShippingDetail> CreateAsync(ShippingDetail shippingDetail);
        Task<ShippingDetail> GetByIdAsync(int id);
        Task<IEnumerable<ShippingDetail>> GetAllAsync();
        Task<ShippingDetail> UpdateAsync(int id, ShippingDetail shippingDetail);
        Task DeleteAsync(int id);
    }

}
