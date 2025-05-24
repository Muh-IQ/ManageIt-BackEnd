using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Business_Tier.Third_party_services.Email_Service
{
    internal class SendGridCredentials
    {
        private static SendGridCredentials _instance;
        private static readonly object _lock = new object();

        private string _apiKey = string.Empty;

        public string ApiKey => _apiKey;
        private SendGridCredentials()
        {
            FillCredentials();
        }

        public static SendGridCredentials Instance { get 
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new SendGridCredentials();
                    }
                }
                return _instance; 
            }
        }

        private void FillCredentials()
        {
            _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new InvalidOperationException("SendGrid API key is missing.");
            }

        }
    }
}
