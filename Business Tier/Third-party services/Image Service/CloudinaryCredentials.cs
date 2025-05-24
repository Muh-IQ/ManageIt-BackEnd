using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier.Cloudinary_Service
{
    internal class CloudinaryCredentials
    {
        private static CloudinaryCredentials _instance;
        private static readonly object _lock = new object();

        private string _cloudName = string.Empty;
        private string _apiSecretKey = string.Empty;
        private string _apiKey = string.Empty;

        public string CloudName => _cloudName;
        public string ApiSecretKey => _apiSecretKey;
        public string ApiKey => _apiKey;

        public Account GetAccount()
        {
            if (string.IsNullOrEmpty(_cloudName) || string.IsNullOrEmpty(_apiSecretKey) || string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("account data are not found.");

            }

            return new Account(_cloudName, _apiKey, _apiSecretKey);
        }
        private CloudinaryCredentials() 
        {
            FillCredentials();
        }

        public static CloudinaryCredentials Instance { get 
            {
                if (_instance == null)
                {
                    lock(_lock)
                    {
                        if(_instance == null)
                            _instance = new CloudinaryCredentials();
                    }
                }
                return _instance; 
            } 
        }
        private void FillCredentials()
        {
            _cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
            _apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
            _apiSecretKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");

            if (string.IsNullOrWhiteSpace(_cloudName) ||
                string.IsNullOrWhiteSpace(_apiKey) ||
                string.IsNullOrWhiteSpace(_apiSecretKey))
            {
                throw new InvalidOperationException("Cloudinary credentials are missing.");
            }
        }

    }
}
