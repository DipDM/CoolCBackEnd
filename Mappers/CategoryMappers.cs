using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Category;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDto ToCategoryDto(this Category categoryModel)
        {
            return new CategoryDto
            {
                CategoryId = categoryModel.CategoryId,
                Name = categoryModel.Name,
                Description = categoryModel.Description
            };
        }

        public static Category ToCategoryFromCreate(this CreateCategoryRequestDto categoryRequestDto)
        {
            return new Category
            {
                Name = categoryRequestDto.Name,
                Description = categoryRequestDto.Description
            };
        }
    }
}