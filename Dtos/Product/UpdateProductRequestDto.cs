using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.Product
{
    public class UpdateProductRequestDto
    {
        [MinLength(3, ErrorMessage = "Model name shoul have at least")]
        [MaxLength(100, ErrorMessage = "Model name should be ove 10 characters")]
        public string? Name { get; set; } = string.Empty;
        [MinLength(3, ErrorMessage = "Model name shoul have at least")]
        [MaxLength(100, ErrorMessage = "Model name should be ove 10 characters")]
        public string? Description { get; set; } = string.Empty;
        [Range(1 , 10000)]
        public decimal? Price { get; set; }
        public int? CategoryId {get; set;}
        public int? BrandId {get; set;}
    }
}