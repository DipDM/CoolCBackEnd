using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CoolCBackEnd.Service
{
    public class OtpCacheService : IOtpCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<OtpCacheService> _logger;

        public OtpCacheService(IMemoryCache cache, ILogger<OtpCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public void StoreOtp(string email, string otp, TimeSpan duration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            };
            _cache.Set(email, otp, cacheEntryOptions);
            _logger.LogInformation("Storing OTP for {Email}", email);
        }

        public string GetOtp(string email)
        {
            _cache.TryGetValue(email, out string otp);
            return otp;
        }


        public bool ValidateOtp(string email, string otp)
        {
            if (_cache.TryGetValue(email, out string storedOtp))
            {
                _logger.LogInformation("Validating OTP for {Email}. Stored OTP: {StoredOtp}, Provided OTP: {Otp}", email, storedOtp, otp);
                return storedOtp == otp;
            }

            return false;
        }
    }
}