using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Models
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public string Error { get; set; }
        public Admin Admin {get; set;}
    }
}
