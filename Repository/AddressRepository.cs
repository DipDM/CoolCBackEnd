using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Data;
using CoolCBackEnd.Dtos.Address;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolCBackEnd.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDBContext _context;

        public AddressRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Address> CreateAsync(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> GetByIdAsync(int addressId)
        {
            return await _context.Addresses.FindAsync(addressId);
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Address> UpdateAsync(int addressId, Address address)
        {
            var existingAddress = await _context.Addresses.FindAsync(addressId);
            if (existingAddress == null) return null;

            existingAddress.City = address.City;
            existingAddress.AddressLine1 = address.AddressLine1;
            existingAddress.AddressLine2 = address.AddressLine2;
            existingAddress.State = address.State;
            existingAddress.Country = address.Country;
            existingAddress.PostalCode = address.PostalCode;

            await _context.SaveChangesAsync();
            return existingAddress;
        }

        public async Task<Address> DeleteAsync(int addressId)
        {
            var address = await _context.Addresses.FindAsync(addressId);
            if (address == null) return null;

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }


}