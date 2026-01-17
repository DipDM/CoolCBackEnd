using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class CouponUser
    {
        public int CouponUserId {get; set;}
        public Guid UserId {get; set;}
        public int CouponId {get; set;}
        public Coupon Coupon{get; set;}
        public DateTime RedeemedDate {get; set;}
        public Guid OrderId{get; set;}
    }
}