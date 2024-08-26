using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId {get; set;}
        public int Quantity {get; set;}
        public int? CartId {get; set;}
        public Cart? Cart {get; set;}
        public int? ProductId {get; set;}
        public Product? Product {get; set;}
    }
}