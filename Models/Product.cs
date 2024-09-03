using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; } 

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        public int? CategoryId {get; set;}
        public Category? Category {get; set;}
        public int? BrandId {get; set;}
        public Brand? Brand {get; set;}
        public List<ProductImage> ProductImages {get; set;} = new List<ProductImage>();
        public List<Comment> Comments {get; set;} = new List<Comment>();
        public ICollection<ProductSize> ProductSizes {get; set;}
    }
}