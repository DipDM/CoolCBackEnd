using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.OrderItem
{
    public class CreateOrderItemRequest
    {
        public int? ProductId {get; set;}
        public Guid? OrderId {get; set;}
        public int Quantity {get; set;}
        public int Price {get; set;} 
    }
}