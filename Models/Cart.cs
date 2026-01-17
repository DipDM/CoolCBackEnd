using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class Cart
    {
        [Key]
        public int CartId {get; set;}
        public Guid UserId {get; set;}
        public User User {get; set;}
        public ICollection<CartItem> CartItems {get; set;} = new List<CartItem>();
        public decimal TotalAmount {get; set;}

    }
}