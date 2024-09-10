using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace CoolCBackEnd.Dtos.CouponUser
{
    public class CouponUserDto
    {
        public string CouponUserId { get; set; }
        public Guid UserId {get; set;}
        public int CouponId {get; set;}
        public DateTime RedeemedDate {get; set;}
        public Guid OrderId {get; set;}
    }
}