using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.Payment
{
    public class PaymentRequestDto
    {
        public Guid OrderId {get; set;}
        public decimal Amount {get; set;}
    }
}