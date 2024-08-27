using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.ShippingDetail;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingDetailController : ControllerBase
    {
        private readonly IShippingDetailRepository _shippingDetailRepository;

        public ShippingDetailController(IShippingDetailRepository shippingDetailRepository)
        {
            _shippingDetailRepository = shippingDetailRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShippingDetailDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shippingDetail = createDto.ToEntity();
            var createdShippingDetail = await _shippingDetailRepository.CreateAsync(shippingDetail);

            return CreatedAtAction(nameof(GetById), new { id = createdShippingDetail.ShippingDetailId }, createdShippingDetail.ToShippingDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shippingDetail = await _shippingDetailRepository.GetByIdAsync(id);
            if (shippingDetail == null)
            {
                return NotFound();
            }

            return Ok(shippingDetail.ToShippingDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shippingDetails = await _shippingDetailRepository.GetAllAsync();
            return Ok(shippingDetails.Select(sd => sd.ToShippingDto()));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateShippingDetailDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingShippingDetail = await _shippingDetailRepository.GetByIdAsync(id);
            if (existingShippingDetail == null)
            {
                return NotFound();
            }

            existingShippingDetail.ApplyUpdates(updateDto);
            var updatedShippingDetail = await _shippingDetailRepository.UpdateAsync(id, existingShippingDetail);

            return Ok(updatedShippingDetail.ToShippingDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var shippingDetail = await _shippingDetailRepository.GetByIdAsync(id);
            if (shippingDetail == null)
            {
                return NotFound();
            }

            await _shippingDetailRepository.DeleteAsync(id);
            return NoContent();
        }
    }

}