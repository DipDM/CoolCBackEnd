using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.Order
{
    public class OrderDto
    {
        public Guid OrderId {get; set;}
        public string OrderStatus {get; set;}
        public string PaymentStatus {get; set;}
        public int TotalAmount {get; set;}
        public Guid UserId {get; set;}
    }
}