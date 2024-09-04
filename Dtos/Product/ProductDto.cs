using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Comment;
using CoolCBackEnd.Dtos.ProductImage;
using CoolCBackEnd.Dtos.ProductSize;

namespace CoolCBackEnd.Dtos.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public int? CategoryId {get; set;}
        public int? BrandId {get; set;}
        public List<ProductImageDto> ProductImages {get; set;}
        public List<CommentDto> Comments {get; set;} 
        public List<ProductSizeDto> ProductSizes {get; set;}
    }
}