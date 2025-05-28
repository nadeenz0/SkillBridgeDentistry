using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
    public interface IOtpService
    {
        
            string GenerateOtp(int length);
            void SaveOtp(string email, string otp);
            string GetOtp(string email);
            void RemoveOtp(string email);
        

    }
}
