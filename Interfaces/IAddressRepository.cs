using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Interfaces
{
    public interface IAddressRepository
{
    Task<Address> CreateAsync(Address address);
    Task<Address> GetByIdAsync(int addressId);
    Task<IEnumerable<Address>> GetAllAsync();
    Task<Address> UpdateAsync(int addressId, Address address);
    Task<Address> DeleteAsync(int addressId);
}

}