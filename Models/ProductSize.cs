using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class ProductSize
    {
        [Key]
        public int ProductSizeId {get; set;}
        public int ProductId {get; set;}
        public int SizeId {get; set;}
        public Product? Product {get; set;}
        public Size? Size {get; set;}
        // public List<Product> Products {get; set;} = new List<Product>();
        // public List<Size> Sizes {get; set;} = new List<Size>();
    }
}