using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId {get; set;}
        public string OrderStatus{get; set;}
        public string PaymentStatus{get; set;}
        public int TotalAmount{get; set;}
        public ICollection<OrderItem> OrderItems{get; set;}
    }
}