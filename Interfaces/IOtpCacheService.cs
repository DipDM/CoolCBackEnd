using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCBackEnd.Interfaces
{
    public interface IOtpCacheService
    {
        void StoreOtp(string email, string otp, TimeSpan duration);
        string GetOtp(string email);
        bool ValidateOtp(string email,string otp);
    }
}