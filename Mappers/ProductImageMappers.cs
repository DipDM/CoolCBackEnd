using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.ProductImage;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class ProductImageMappers
    {
        public static ProductImageDto ToProductImageDto(this ProductImage productImageModel)
        {
            return new ProductImageDto
            {
                ProductImageId = productImageModel.ProductImageId,
                ImagePath = productImageModel.ImagePath
            };
        }

        public static ProductImage ToPRoductImageFromCreate(this CreateProductImageDto productImageDto, int ProductId)
        {
            return new ProductImage
            {
                ImagePath = productImageDto.ImagePath,
                ProductId = ProductId
            };
        }

        public static ProductImage ToCommentFromUpdate(this UpdateProductImageRequestDto productImageDto)
        {
            return new ProductImage
            {
                ImagePath = productImageDto.ImagePath
            };
        }
    }
}