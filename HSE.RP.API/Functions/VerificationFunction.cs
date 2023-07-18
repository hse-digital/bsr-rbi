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
    public class VerificationFunction
    {

        private readonly OTPService otpService;
        private readonly DynamicsService dynamicsService;
        private readonly FeatureOptions featureOptions;
        private readonly NotificationService notificationService;

        public VerificationFunction(DynamicsService dynamicsService , OTPService otpService, IOptions<FeatureOptions> featureOptions, NotificationService notificationService)
        {
            this.dynamicsService = dynamicsService;
            this.notificationService = notificationService;
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
            await notificationService.SendOTPEmail(emailVerificationModel.EmailAddress, otpToken: otpToken);

            return new CustomHttpResponseData
            {
                HttpResponse = request.CreateResponse()
            };
        }

        [Function(nameof(SendVerificationSms))]
        public async Task<CustomHttpResponseData> SendVerificationSms([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
        {
            var phoneVerificationModel = await request.ReadAsJsonAsync<PhoneNumberVerificationModel>();
            var validation = phoneVerificationModel.Validate();
            if (!validation.IsValid)
            {
                return await request.BuildValidationErrorResponseDataAsync(validation);
            }

            var otpToken = otpService.GenerateToken(phoneVerificationModel.PhoneNumber);
            await notificationService.SendOTPSms(phoneVerificationModel.PhoneNumber, otpToken: otpToken);

            return new CustomHttpResponseData
            {
                HttpResponse = request.CreateResponse()
            };
        }

        [Function(nameof(ValidateOTPToken))]
        public async Task<CustomHttpResponseData> ValidateOTPToken([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
        {
            var otpValidationModel = await request.ReadAsJsonAsync<OTPValidationModel>();
#if DEBUG
            if(otpValidationModel.OTPToken == "111111")
            {
                return new CustomHttpResponseData
                {
                    HttpResponse = request.CreateResponse(HttpStatusCode.OK)
                };
            }
#endif
            var isTokenValid = otpValidationModel.Validate().IsValid && otpService.ValidateToken(otpValidationModel.OTPToken, otpValidationModel.Data);

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
