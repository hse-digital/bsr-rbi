using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtpNet;

namespace HSE.RP.API.Services
{
    public class OTPService
    {
        public virtual string GenerateToken(string secretKey, DateTime? baseDateTime = null)
        {
            var totp = GetTotp(secretKey);
            return totp.ComputeTotp(baseDateTime ?? DateTime.UtcNow);
        }

        public virtual bool ValidateToken(string otpToken, string secretKey)
        {
            var totp = GetTotp(secretKey);
            return totp.VerifyTotp(otpToken, out _);
        }

        protected Totp GetTotp(string secretKey)
        {
            return new Totp(Encoding.UTF8.GetBytes(secretKey), step: 60 * 60);
        }
    }
}
