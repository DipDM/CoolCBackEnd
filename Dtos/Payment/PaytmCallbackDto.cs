using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.Payment
{
    public class PaytmCallbackDto
    {
        public Guid OrderId {get; set;}
        public string TransactionId {get; set;}
        public decimal Amount {get; set;}
        public string Status {get; set;} //Success,Failed,Pending
    }
}