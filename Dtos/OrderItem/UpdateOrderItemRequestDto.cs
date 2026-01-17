using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.OrderItem
{
    public class UpdateOrderItemRequestDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}