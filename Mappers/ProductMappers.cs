using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Product;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class ProductMappers
    {
        public static ProductDto ToProductDto(this Product productModel)
        {
            return new ProductDto 
            {
                ProductId = productModel.ProductId,
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                CategoryId = productModel.CategoryId,
                BrandId = productModel.BrandId,
                ProductImages = productModel.ProductImages.Select( c => c.ToProductImageDto()).ToList(),
                Comments = productModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static Product ToProductFromCreateDTO(this CreateProductRequestDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                BrandId = productDto.BrandId
            };
        }
    }
}