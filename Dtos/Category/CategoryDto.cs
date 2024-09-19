using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Product;

namespace CoolCBackEnd.Dtos.Category
{
    public class CategoryDto
    {
        public int CategoryId {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public List<ProductDto> Products {get; set;}

    }
}