using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.CouponUser
{
    public class CouponUserCreateDto
    {
        public Guid UserId {get; set;}
        public int CouponId {get; set;}
        public DateTime RedeemedDate {get; set;}
        public Guid OrderId {get; set;}
    }
}