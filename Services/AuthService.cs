using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using ProtectorTokenAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProtectorTokenAuth.Services
{
    public class AuthService : IAuthService
    {
        public int TokenExpireTimeDays { get; set; }

        private string dataProtectorName;
        private IDataProtector dataProtector;

        public AuthService(IDataProtectionProvider protectionProvider)
        {
            TokenExpireTimeDays = Constants.DEFAULT_TOKEN_EXPIRATION_DAYS;

            this.dataProtectorName = Constants.DEFAULT_DATA_PROTECTION_PROVIDER_NAME;
            this.dataProtector = protectionProvider.CreateProtector(dataProtectorName);
        }

        public AuthService(IDataProtectionProvider protectionProvider, string dataProtectorName, int tokenExpirationDays)
        {
            TokenExpireTimeDays = tokenExpirationDays;

            this.dataProtectorName = dataProtectorName;
            this.dataProtector = protectionProvider.CreateProtector(dataProtectorName);
        }

        public string CreateAccessToken(TokenPayload payload)
        {
            string json = JsonConvert.SerializeObject(payload);
            return dataProtector.Protect(json);
        }

        public bool ValidateAccessToken(string accessToken, string[] roles)
        {
            if (accessToken == null)
                return false;

            try
            {
                string json = dataProtector.Unprotect(accessToken);
                TokenPayload payload = JsonConvert.DeserializeObject<TokenPayload>(json);

                if (payload == null || (roles != null && roles.Length > 0 &&
                        !roles.Any(x => payload.UserRoles.Contains(x))) || DateTime.UtcNow > payload.ExpiresAt)
                    return false;
                else
                {
                    ActiveUser = payload.Username;
                    ActiveUserRoles = payload.UserRoles;
                    ActiveUserData = payload.CustomData;
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public string HashPassword(string password, string salt = null)
        {
            StringBuilder sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                if (salt != null)
                {
                    // salt password
                    password = salt + password;
                }

                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(password));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public string ActiveUser { get; private set; }
        public string[] ActiveUserRoles { get; private set; }
        public Dictionary<string, string> ActiveUserData { get; private set; }
    }
}
