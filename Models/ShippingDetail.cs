using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class ShippingDetail
    {
        public int ShippingDetailId { get; set; }
        public string ShippingStatus { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime EstimatedDelivery { get; set; }
        public Guid OrderId { get; set; } 
        public Order Order { get; set; }
        public int AddressId { get; set; } 
        public Address Address { get; set; }
    }
}