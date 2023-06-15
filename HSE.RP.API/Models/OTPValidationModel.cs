using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models
{
    public record OTPValidationModel(string OTPToken, string EmailAddress) : IValidatableModel
    {
        public ValidationSummary Validate()
        {
            var hasErrors = string.IsNullOrEmpty(OTPToken) || string.IsNullOrEmpty(EmailAddress);
            return new ValidationSummary(!hasErrors, Array.Empty<string>());
        }
    }
}
