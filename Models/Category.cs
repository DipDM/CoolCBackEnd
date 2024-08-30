using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public ICollection<Product> Products {get; set;}
        
    }
}