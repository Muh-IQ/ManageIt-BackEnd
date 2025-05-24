using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Tier.Service
{
    public interface IEmailService
    {
        Task<bool> SendOrderCompletionAsync(string Email, int orderID);
        Task<bool> SendOtpAsync(string toEmail, string otpCode);
    }
}
