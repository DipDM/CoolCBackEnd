using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.ProductSize;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class ProductSizeMappers
    {
        public static ProductSizeDto ToProductSizeDto(this ProductSize productSize)
        {
            return new ProductSizeDto
            {
                ProductSizeId = productSize.ProductSizeId,
                ProductId = productSize.ProductId,
                SizeId = productSize.SizeId,
            };
        }

        public static ProductSize ToCreateProductSize(this CreateProductSizeDto createProductSizeDto)
        {
            return new ProductSize
            {
                ProductId = createProductSizeDto.ProductId,
                SizeId = createProductSizeDto.SizeId,
            };
        }
    }
}