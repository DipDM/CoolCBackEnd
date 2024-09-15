using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.User
{
    public class UpdateUserDto
    {
        public string UserId { get; set; }
        public string NewUsername { get; set; }
        public string NewPhoneNumber { get; set; }
    }
}