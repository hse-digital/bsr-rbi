namespace HSE.RP.API.Models
{
    public record OTPValidationModel(string OTPToken, string Data) : IValidatableModel
    {
        public ValidationSummary Validate()
        {
            var hasErrors = string.IsNullOrEmpty(OTPToken) || string.IsNullOrEmpty(Data);
            return new ValidationSummary(!hasErrors, Array.Empty<string>());
        }
    }
}
