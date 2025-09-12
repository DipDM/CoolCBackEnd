using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(IBrandRepository brandRepo, IWebHostEnvironment webHostEnvironment)
        {
            _brandRepo = brandRepo;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Create([FromForm] CreateBrandRequestDto brandCreateDto, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string imageFileName = null;

            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                // Sanitize the brand name to create a valid folder and file name
                string sanitizedBrandName = Regex.Replace(brandCreateDto.Name.ToLower(), @"[^a-z0-9]", "");

                // Ensure the file extension is preserved
                string fileExtension = Path.GetExtension(imageFile.FileName);
                
                // add both sanitized + fileextesnion
                imageFileName = sanitizedBrandName + fileExtension;

                // Generate the folder path for the brand
                var brandFolder = Path.Combine(_webHostEnvironment.WebRootPath, "brand", sanitizedBrandName);

                // Generate the file path inside the brand's folder
                var filePath = Path.Combine(brandFolder, imageFileName);

                // Create folder if it doesn't exist
                if (!Directory.Exists(brandFolder))
                {
                    Directory.CreateDirectory(brandFolder);
                }

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }

            // Create the brand model
            var brandModel = new Brand
            {
                Name = brandCreateDto.Name,
                NickName = brandCreateDto.NickName,
                Image = imageFileName // Save the image file name in the database
            };

            var createdBrand = await _brandRepo.CreateAsync(brandModel);
            return CreatedAtAction(nameof(GetById), new { BrandId = createdBrand.BrandId }, createdBrand);
        }


        [HttpPut("{BrandId:int}")]
        public async Task<IActionResult> Update(int BrandId, [FromForm] UpdateBrandRequestDto updateBrandDto, IFormFile imageFile)
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

            // Update Name and NickName if provided
            if (!string.IsNullOrEmpty(updateBrandDto.Name))
            {
                existingBrand.Name = updateBrandDto.Name;
            }
            if (!string.IsNullOrEmpty(updateBrandDto.NickName))
            {
                existingBrand.NickName = updateBrandDto.NickName;
            }

            // Handle Image Upload
            if (imageFile != null && imageFile.Length > 0)
            {
                // Sanitize the brand name to create a valid folder and file name
                string sanitizedBrandName = Regex.Replace(existingBrand.Name.ToLower(), @"[^a-z0-9]", "");

                // Ensure the file extension is preserved
                string fileExtension = Path.GetExtension(imageFile.FileName);
                string newFileName = sanitizedBrandName + fileExtension;

                // Generate the folder path for the brand
                var brandFolder = Path.Combine(_webHostEnvironment.WebRootPath, "brand", sanitizedBrandName);

                // Generate the file path inside the brand's folder
                var filePath = Path.Combine(brandFolder, newFileName);

                // Create folder if it doesn't exist
                if (!Directory.Exists(brandFolder))
                {
                    Directory.CreateDirectory(brandFolder);
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingBrand.Image))
                {
                    var oldImagePath = Path.Combine(brandFolder, existingBrand.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the new image file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                // Update the brand image field
                existingBrand.Image = newFileName;
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
