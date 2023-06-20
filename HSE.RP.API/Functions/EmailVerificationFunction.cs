using System.Net;
using HSE.RP.API.Extensions;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Functions
{
    public class EmailVerificationFunction
    {

        private readonly OTPService otpService;
        private readonly DynamicsService dynamicsService;
        private readonly FeatureOptions featureOptions;

        public EmailVerificationFunction(DynamicsService dynamicsService , OTPService otpService, IOptions<FeatureOptions> featureOptions)
        {
            this.dynamicsService = dynamicsService;
            this.otpService = otpService;
            this.featureOptions = featureOptions.Value;
        }

        [Function(nameof(SendVerificationEmail))]
        public async Task<CustomHttpResponseData> SendVerificationEmail([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
        {
            var emailVerificationModel = await request.ReadAsJsonAsync<EmailVerificationModel>();
            var validation = emailVerificationModel.Validate();
            if (!validation.IsValid)
            {
                return await request.BuildValidationErrorResponseDataAsync(validation);
            }

            var otpToken = otpService.GenerateToken(emailVerificationModel.EmailAddress);
            await dynamicsService.SendVerificationEmail(emailVerificationModel, otpToken);
            return new CustomHttpResponseData
            {
                HttpResponse = request.CreateResponse()
            };
        }

        [Function(nameof(ValidateOTPToken))]
        public async Task<CustomHttpResponseData> ValidateOTPToken([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
        {
            var otpValidationModel = await request.ReadAsJsonAsync<OTPValidationModel>();
            var isTokenValid = otpValidationModel.Validate().IsValid && otpService.ValidateToken(otpValidationModel.OTPToken, otpValidationModel.EmailAddress);

            var returnStatusCode = HttpStatusCode.OK;
            if (!featureOptions.DisableOtpValidation && !isTokenValid)
            {
                returnStatusCode = HttpStatusCode.BadRequest;
            }

            return new CustomHttpResponseData
            {
                HttpResponse = request.CreateResponse(returnStatusCode)
            };
        }

    }
}
