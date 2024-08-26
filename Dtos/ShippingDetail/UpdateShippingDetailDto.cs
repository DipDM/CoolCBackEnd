using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.ShippingDetail
{
    public class UpdateShippingDetailDto
    {
        public string ShippingStatus { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime? EstimatedDelivery { get; set; }  // Nullable for optional updates
    }
}