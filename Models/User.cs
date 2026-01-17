using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CoolCBackEnd.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid UserId => base.Id;
        public ICollection<Comment> Comments {get; set;}
        public ICollection<Address> Addresses {get; set;}
        public ICollection<Cart> Carts {get; set;}
        public ICollection<Order> Orders {get; set;}
        public DateTime CreationTime {get; set;}
    }
}