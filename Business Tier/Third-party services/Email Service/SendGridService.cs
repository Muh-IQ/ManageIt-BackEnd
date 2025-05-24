using Interface_Tier.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Business_Tier.Third_party_services.Email_Service;

namespace Business_Tier.Email_Service
{
    public class SendGridService : IEmailService
    {
        private readonly string _senderEmail;

        public SendGridService(IConfiguration configuration)
        {
            _senderEmail = configuration["SendGrid:SenderEmail"];
        }

        public async Task<bool> SendOrderCompletionAsync(string Email, int orderID)
        {
            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(_senderEmail, "O&M Store"), // يمكنك تغيير الاسم
                Subject = "Thank you for your purchase!",
                PlainTextContent = $"Thank you for your order #{orderID} placed on {DateTime.Now:MMMM dd, yyyy}. We appreciate your business!",
                HtmlContent = $@"
                <div style='font-family: Arial, sans-serif; line-height:1.6'>
                    <h2 style='color: #2e6c80;'>Thank you for your purchase!</h2>
                    <p>Hello,</p>
                    <p>We’re happy to let you know that your order <strong>#{orderID}</strong> was successfully completed on <strong>{DateTime.Now:MMMM dd, yyyy}</strong>.</p>
                    <p>We appreciate your trust in us. If you have any questions, feel free to reach out at any time.</p>
                    <p>Best regards,<br/>Shoes Store Team</p>
                </div>"
            };

            bool res = await _SendMessageAsync(Email, msg);

            if (!res)
            {
                return false;
                throw new Exception("Failed to send Order Completion email.");
            }

            return true;
        }

        public async Task<bool> SendOtpAsync(string Email, string otpCode)
        {
            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(_senderEmail, "ManageIt"),
                Subject = "Your OTP Code",
                PlainTextContent = $"Your OTP code is: {otpCode}",
                HtmlContent = $"<strong>Your OTP code is: {otpCode}</strong>"
            };
            bool res = await _SendMessageAsync(Email, msg);

            if (!res)
            {
                return false;
                throw new Exception("Failed to send OTP email.");
            }

            return true;
        }
        private async Task<bool> _SendMessageAsync(string Email, SendGridMessage Message)
        {
            var client = new SendGridClient(SendGridCredentials.Instance.ApiKey);

            Message.AddTo(new EmailAddress(Email));

            var response = await client.SendEmailAsync(Message);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}
