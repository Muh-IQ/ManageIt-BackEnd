using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface IOTPService
    {
        Task<bool> AddOTP(string email);
        Task<bool> VerifyOTP(string email, string otp);
        string GenerateNumericOTP(int length = 6);
    }
}
