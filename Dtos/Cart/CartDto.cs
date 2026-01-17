using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.CartItem;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Dtos.Cart
{
    public class CartDto
    {
        public int CartId {get; set;}
        public Guid UserId {get; set;}
        public decimal TotalAmount {get; set;}
    }
}