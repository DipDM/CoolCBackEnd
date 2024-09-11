using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class BillingReport
    {
        public Guid OrderId {get; set;}
        public string PaymentMethod {get; set;}
        public string TransactionId {get; set;}
        public decimal TotalAmount {get; set;}
        public decimal AmountPaid {get; set;}
        public DateTime PaymentDate {get; set;}
        public string PaymentStatus {get; set;}
    }
}