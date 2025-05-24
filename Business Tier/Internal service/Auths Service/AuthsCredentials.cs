using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier.Internal_service.Auths_Service
{
    public class AuthsCredentials
    {
        private static AuthsCredentials _instance;
        private static readonly object _lock = new object();

        public string SecretKey { get; private set; }
        public string Issuer { get; private set; }
        public string Audience { get; private set; }
        public byte AccessTokenMinutes { get; private set; }
        public byte RefreshTokenDays { get; private set; }

        public static AuthsCredentials Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new AuthsCredentials();
                    }
                }

                return _instance;
            }
        }
        private AuthsCredentials()
        {
            GetCredentials();
        }

        private void GetCredentials()
        {
            SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            AccessTokenMinutes = byte.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_TOKEN_EXPIRATION_MINUTES"));
            RefreshTokenDays = byte.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_EXPIRATION_DAYS"));
        }
    }
}
