using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Category;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryRepository categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var category = await _categoryRepo.GetCategoriesWithProductsAsync();
            if (category == null || !category.Any())
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet("{CategoryId:int}")]
        public async Task<IActionResult> GetById(int CategoryId)
        {
            var category = await _categoryRepo.GetCategoriesWithProductsAsync();

            var categoryDto = category.FirstOrDefault(b => b.CategoryId == CategoryId);

            if (category == null)
            {
                return NotFound();
            }
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryRequestDto categoryCreateDto, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string imageFileName = null;

            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                // Sanitize the category name to create a valid folder and file name
                string sanitizedCategoryName = Regex.Replace(categoryCreateDto.Name.ToLower(), @"[^a-z0-9]", "");

                // Ensure the file extension is preserved
                string fileExtension = Path.GetExtension(imageFile.FileName);
                imageFileName = sanitizedCategoryName + fileExtension;

                // Generate the folder path for the category
                var categoryFolder = Path.Combine(_webHostEnvironment.WebRootPath, "category", sanitizedCategoryName);

                // Generate the file path inside the category's folder
                var filePath = Path.Combine(categoryFolder, imageFileName);

                // Create folder if it doesn't exist
                if (!Directory.Exists(categoryFolder))
                {
                    Directory.CreateDirectory(categoryFolder);
                }

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }

            // Create the category model
            var categoryModel = new Category
            {
                Name = categoryCreateDto.Name,
                Description = categoryCreateDto.Description,
                Image = imageFileName // Save the image file name in the database
            };

            var createdCategory = await _categoryRepo.CreateAsync(categoryModel);
            return CreatedAtAction(nameof(GetById), new { CategoryId = createdCategory.CategoryId }, createdCategory);
        }


        [HttpPut("{CategoryId:int}")]
        public async Task<IActionResult> Update(int CategoryId, [FromForm] UpdateCategoryRequestDto updateCategoryDto, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _categoryRepo.GetByIdAsync(CategoryId);
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Update Name and Description if provided
            if (!string.IsNullOrEmpty(updateCategoryDto.Name))
            {
                existingCategory.Name = updateCategoryDto.Name;
            }
            if (!string.IsNullOrEmpty(updateCategoryDto.Description))
            {
                existingCategory.Description = updateCategoryDto.Description;
            }

            // Handle Image Upload
            if (imageFile != null && imageFile.Length > 0)
            {
                // Sanitize the category name to create a valid folder and file name
                string sanitizedCategoryName = Regex.Replace(existingCategory.Name.ToLower(), @"[^a-z0-9]", "");

                // Ensure the file extension is preserved
                string fileExtension = Path.GetExtension(imageFile.FileName);
                string newFileName = sanitizedCategoryName + fileExtension;

                // Generate the folder path for the category
                var categoryFolder = Path.Combine(_webHostEnvironment.WebRootPath, "category", sanitizedCategoryName);

                // Generate the file path inside the category's folder
                var filePath = Path.Combine(categoryFolder, newFileName);

                // Create folder if it doesn't exist
                if (!Directory.Exists(categoryFolder))
                {
                    Directory.CreateDirectory(categoryFolder);
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingCategory.Image))
                {
                    var oldImagePath = Path.Combine(categoryFolder, existingCategory.Image);
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

                // Update the category image field
                existingCategory.Image = newFileName;
            }

            var updatedCategory = await _categoryRepo.UpdateAsync(CategoryId, existingCategory);
            return Ok(updatedCategory);
        }



        [HttpDelete("{CategoryId:int}")]
        public async Task<IActionResult> Delete(int CategoryId)
        {
            var deletedCategory = await _categoryRepo.DeleteAsync(CategoryId);

            if (deletedCategory == null)
            {
                return NotFound();
            }

            return Ok(deletedCategory);
        }
    }
}