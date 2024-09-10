using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.CouponOrder
{
    public class CouponOrderUpdateDto
    {
        public int CouponId { get; set; }
        public Guid OrderId { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}