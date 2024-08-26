using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.ShippingDetail
{
    public class ShippingDetailDto
    {
        public int ShippingDetailId { get; set; }
        public int OrderId { get; set; }
        public int AddressId { get; set; }
        public string ShippingStatus { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime EstimatedDelivery { get; set; }
    }
}