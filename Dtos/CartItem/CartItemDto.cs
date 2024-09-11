using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.CartItem
{
    public class CartItemDto
    {
        public int CartItemId {get; set;}
        public int ProductId {get; set;}
        public int CartId {get; set;}
        public int Quantity {get; set;} 
    }
}