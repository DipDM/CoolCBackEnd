using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CoolCBackEnd.Models
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string Description { get; set; }
    }
}