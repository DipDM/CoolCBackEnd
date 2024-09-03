using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.ProductSize
{
    public class ProductSizeDto
    {
        public int ProductSizeId{get; set;}
        public int ProductId {get; set;}
        public int SizeId {get; set;}
    }
}