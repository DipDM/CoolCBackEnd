using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.ProductImage
{
    public class ProductImageDto
    {
        public int ProductImageId {get; set;}
        public string ImagePath {get; set;}
        public int? ProductId {get; set;}
    }
}