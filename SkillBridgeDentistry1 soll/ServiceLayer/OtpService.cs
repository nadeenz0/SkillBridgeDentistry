using CoreLayer.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class OtpService:IOtpService
    {
        private readonly IMemoryCache _memoryCache;

        public OtpService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string GenerateOtp(int length)
        {
            var random = new Random();
            return string.Join("", Enumerable.Range(0, length).Select(_ => random.Next(0, 10)));
        }

        public void SaveOtp(string email, string otp)
        {
            _memoryCache.Set(email, otp, TimeSpan.FromMinutes(5)); // مدة الصلاحية
        }

        public string GetOtp(string email)
        {
            _memoryCache.TryGetValue(email, out string otp);
            return otp;
        }

        public void RemoveOtp(string email)
        {
            _memoryCache.Remove(email);
        }
    }
}
