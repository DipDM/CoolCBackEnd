using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class Payment
    {
        public Guid PaymentId {get; set;}
        public Guid OrderId{get; set;}
        public Order Order {get; set;}
        public string PaymentMethod {get; set;}  //Paytm , RazorPay etc
        public string TransactionId {get; set;}
        public string PaymentStatus {get; set;} // Success , Failed , Pending , etc
        public decimal AmountPaid {get; set;} //Total amount
        public DateTime PaymentDate{get; set;} //Date of payment
    }
}