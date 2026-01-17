using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Dtos.User
{
    public class OtpVerificationDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

}