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
        public string OrderStatus{get; set;} = "Pending";
        public string PaymentStatus{get; set;} = "Pending";
        public Payment Payment {get; set;}
        public DateTime OrderDate {get; set;} = DateTime.Now;
        public decimal TotalAmount{get; set;}
        public Guid UserId {get; set;}
        public User User {get; set;}
        public ICollection<OrderItem> OrderItems{get; set;}
        public ICollection<ShippingDetail> ShippingDetails {get; set;}
        public ICollection<Payment> Payments {get; set;}
    }
}