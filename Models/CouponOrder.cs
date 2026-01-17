using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class CouponOrder
    {
        public int CouponOrderId { get; set; }
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; }
        public Guid OrderId { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}