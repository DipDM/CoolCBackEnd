using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Size;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class SizeMappers
    {
        public static SizeDto ToSizeDto(this Size size)
        {
            return new SizeDto
            {
                SizeId = size.SizeId,
                SizeName = size.SizeName,
            };
        }
        public static Size ToCreateSize(this SizeCreateDto sizeCreateDto)
        {
            return new Size
            {
                SizeName = sizeCreateDto.SizeName,
            };
        }
    }
}