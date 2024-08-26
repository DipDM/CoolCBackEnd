using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.Product
{
    public class CreateProductRequestDto
    {
        [Required]
        [MaxLength(1000, ErrorMessage = "sfgaskgjkasnfg")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage =" asdasnjddjfbsaf fsabgkjasfgj")]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(1,10000)]
        public decimal Price { get; set; }
        public int CategoryId {get; set;}
        public int BrandId {get; set;}
    }
}