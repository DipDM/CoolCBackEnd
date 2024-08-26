using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class ShippingDetail
    {
        public int ShippingDetailId { get; set; }  // Primary Key
        public int OrderId { get; set; }           // Foreign Key
        public int AddressId { get; set; }         // Foreign Key
        public string ShippingStatus { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime EstimatedDelivery { get; set; }

        // Navigation properties
        public Order? Order { get; set; }
        public Address? Address { get; set; }
    }
}