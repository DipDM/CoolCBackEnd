using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Brand;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class BrandMappers
    {
        public static BrandDto ToCategoryDto(this Brand brandModel)
        {
            return new BrandDto
            {
                BrandId = brandModel.BrandId,
                Name = brandModel.Name,
                NickName = brandModel.NickName
            };
        }

        public static Brand ToBrandFromCreate(this CreateBrandRequestDto brandRequestDto)
        {
            return new Brand
            {
                Name = brandRequestDto.Name,
                NickName = brandRequestDto.NickName
            };
        }
    }
}