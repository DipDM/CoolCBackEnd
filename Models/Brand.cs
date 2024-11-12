using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class Brand
    {
        [Key]
        public int BrandId {get; set;}
        public string Name {get; set;}
        public string NickName {get; set;}
        public string Image {get; set;}
        public ICollection<Product> Products {get; set;}
    }
}