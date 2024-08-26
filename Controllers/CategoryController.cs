using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Category;
using CoolCBackEnd.Interfaces;
using CoolCBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolCBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var category = await _categoryRepo.GetAllAsync();
            return Ok(category);
        }

        [HttpGet("{CategoryId:int}")]
        public async Task<IActionResult> GetById(int CategoryId)
        {
            var category = await _categoryRepo.GetByIdAsync(CategoryId);

            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryRequestDto categorycreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryModel = new Category
            {
                Name = categorycreateDto.Name,
                Description = categorycreateDto.Description
            };
            var createdCategory = await _categoryRepo.CreateAsync(categoryModel);
            return CreatedAtAction(nameof(GetById),new { CategoryId = createdCategory.CategoryId}, createdCategory);
        }

        [HttpPut("{CategoryId:int}")]
        public async Task<IActionResult> Update(int CategoryId,[FromForm] UpdateCategoryRequestDto updatecategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCategory = await _categoryRepo.GetByIdAsync(CategoryId);
            if(existingCategory == null)
            {
                return NotFound();
            }

            if(!string.IsNullOrEmpty(updatecategoryDto.Name))
            {
                existingCategory.Name = updatecategoryDto.Name;
            }
            if(!string.IsNullOrEmpty(updatecategoryDto.Description))
            {
                existingCategory.Description = updatecategoryDto.Description;
            }
            var updatedCategory = await _categoryRepo.UpdateAsync(CategoryId,existingCategory);
            return Ok(updatedCategory);
        }

        [HttpDelete("{CategoryId:int}")]
        public async Task<IActionResult> Delete(int CategoryId)
        {
            var deletedCategory = await _categoryRepo.DeleteAsync(CategoryId);

            if(deletedCategory == null)
            {
                return NotFound();
            }

            return Ok(deletedCategory);
        }
    }
}