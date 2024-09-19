using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Address;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAddressDto createDto)
        {
            try
            {
                var address = new Address
                {
                    UserId = createDto.UserId,
                    AddressLine1 = createDto.AddressLine1,
                    AddressLine2 = createDto.AddressLine2,
                    City = createDto.City,
                    State = createDto.State,
                    Country = createDto.Country,
                    PostalCode = createDto.PostalCode

                };

                await _addressRepository.CreateAsync(address);
                return Ok("Address created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"dsaasd : {ex.Message}");
            }
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetById(int addressId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null) return NotFound();
            return Ok(address.ToAddressDto());
        }

        [HttpGet("get-by-user/{userId}")]
        public async Task<IActionResult> GetAddressesByUserId(Guid userId)
        {
            try
            {
                // Step 1: Fetch all addresses by userId
                var addresses = await _addressRepository.GetAddressesByUserIdAsync(userId);

                // Step 2: Check if addresses exist for the given userId
                if (addresses == null || addresses.Count == 0)
                {
                    return Ok(new { Message = "No addresses found for the specified user." });
                }

                // Step 3: Return the list of addresses
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                // Handle any errors and log the exception if necessary
                return StatusCode(500, new { Error = ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _addressRepository.GetAllAsync();
            return Ok(addresses.Select(a => a.ToAddressDto()));
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> Update(int addressId, UpdateAddressDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAddress = await _addressRepository.GetByIdAsync(addressId);
            if (existingAddress == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updateDto.AddressLine1))
            {
                existingAddress.AddressLine1 = updateDto.AddressLine1;
            }
            if (!string.IsNullOrEmpty(updateDto.AddressLine2))
            {
                existingAddress.AddressLine2 = updateDto.AddressLine2;
            }
            if (!string.IsNullOrEmpty(updateDto.City))
            {
                existingAddress.City = updateDto.City;
            }
            if (!string.IsNullOrEmpty(updateDto.State))
            {
                existingAddress.State = updateDto.State;
            }
            if (!string.IsNullOrEmpty(updateDto.Country))
            {
                existingAddress.Country = updateDto.Country;
            }
            if (!string.IsNullOrEmpty(updateDto.PostalCode))
            {
                existingAddress.PostalCode = updateDto.PostalCode;
            }

            var updatedAddress = await _addressRepository.UpdateAsync(addressId, existingAddress);
            if (updatedAddress == null) return NotFound();
            return Ok(updatedAddress.ToAddressDto());
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> Delete(int addressId)
        {
            var deletedAddress = await _addressRepository.DeleteAsync(addressId);
            if (deletedAddress == null) return NotFound();
            return Ok(deletedAddress);
        }
    }

}