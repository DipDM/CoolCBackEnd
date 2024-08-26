using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId {get; set;}
        public int Quantity {get; set;}
        public int Price {get; set;}
        public Guid? OrderId {get; set;}
        public Order? Order {get; set;}
        public int? ProductId {get; set;}
        public Product? Product {get; set;}
    }
}