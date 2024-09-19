using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Brand;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandRepo;

        public BrandController(IBrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
        }

        // Updated GetAll method to return brands with their products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandRepo.GetBrandsWithProductsAsync();  // Fetching brands with products
            if (brands == null || !brands.Any())
            {
                return NotFound();
            }
            return Ok(brands);  // Return BrandDto objects with products
        }

        // Updated GetById method to include products
        [HttpGet("{BrandId:int}")]
        public async Task<IActionResult> GetById(int BrandId)
        {
            var brand = await _brandRepo.GetBrandsWithProductsAsync();

            var brandDto = brand.FirstOrDefault(b => b.BrandId == BrandId);  // Fetch the specific brand with products

            if (brandDto == null)
            {
                return NotFound();
            }
            return Ok(brandDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandRequestDto brandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var brandModel = new Brand
            {
                Name = brandDto.Name,
                NickName = brandDto.NickName
            };

            var createdBrand = await _brandRepo.CreateAsync(brandModel);
            return CreatedAtAction(nameof(GetById), new { BrandId = createdBrand.BrandId }, createdBrand);
        }

        [HttpPut("{BrandId:int}")]
        public async Task<IActionResult> Update(int BrandId, UpdateBrandRequestDto brandupdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingBrand = await _brandRepo.GetByIdAsync(BrandId);
            if (existingBrand == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(brandupdateDto.Name))
            {
                existingBrand.Name = brandupdateDto.Name;
            }
            if (!string.IsNullOrEmpty(brandupdateDto.NickName))
            {
                existingBrand.NickName = brandupdateDto.NickName;
            }

            var updatedBrand = await _brandRepo.UpdateAsync(BrandId, existingBrand);
            return Ok(updatedBrand);
        }

        [HttpDelete("{BrandId:int}")]
        public async Task<IActionResult> Delete(int BrandId)
        {
            var deletedBrand = await _brandRepo.DeleteAsync(BrandId);

            if (deletedBrand == null)
            {
                return NotFound();
            }
            return Ok(deletedBrand);
        }
    }
}
