using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImageId {get; set;}
        public string ImagePath { get; set; }
        public int ProductId {get; set;}
        [JsonIgnore]
        public Product? Product {get; set;}
    }
}