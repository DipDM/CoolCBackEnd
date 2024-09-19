using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Product;

namespace CoolCBackEnd.Dtos.Brand
{
    public class BrandDto
    {
        public int BrandId {get; set;}
        public string Name {get; set;}
        public string NickName {get; set;}
        public List<ProductDto> Products{get; set;}
    }
}