using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Address;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using CoolCBackEnd.Models;
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
        public async Task<IActionResult> Create([FromForm] CreateAddressDto createDto)
        {
            var address = createDto.ToCreateFromAddress();
            var createdAddress = await _addressRepository.CreateAsync(address);
            return CreatedAtAction(nameof(GetById), new { addressId = createdAddress.AddressId }, createdAddress.ToAddressDto());
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetById(int addressId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null) return NotFound();
            return Ok(address.ToAddressDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _addressRepository.GetAllAsync();
            return Ok(addresses.Select(a => a.ToAddressDto()));
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> Update(int addressId, [FromForm] UpdateAddressDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAddress = await _addressRepository.GetByIdAsync(addressId);
            if(existingAddress == null)
            {
                return NotFound();
            }

            if(!string.IsNullOrEmpty(updateDto.AddressLine1))
            {
                existingAddress.AddressLine1 = updateDto.AddressLine1;
            }
            if(!string.IsNullOrEmpty(updateDto.AddressLine2))
            {
                existingAddress.AddressLine2 = updateDto.AddressLine2;
            }
            if(!string.IsNullOrEmpty(updateDto.City))
            {
                existingAddress.City = updateDto.City;
            }
            if(!string.IsNullOrEmpty(updateDto.State))
            {
                existingAddress.State = updateDto.State;
            }
            if(!string.IsNullOrEmpty(updateDto.Country))
            {
                existingAddress.Country = updateDto.Country;
            }
            if(!string.IsNullOrEmpty(updateDto.PostalCode))
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