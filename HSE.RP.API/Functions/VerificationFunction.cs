using System.Net;
using AutoMapper.Internal;
using HSE.RP.API.Extensions;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using Microsoft.AspNetCore.Http;
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

        public VerificationFunction(DynamicsService dynamicsService, OTPService otpService, IOptions<FeatureOptions> featureOptions, NotificationService notificationService)
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

            var otpToken = await otpService.GenerateToken(emailVerificationModel.EmailAddress.ToUpper());
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

            var otpToken = await otpService.GenerateToken(phoneVerificationModel.PhoneNumber);
            await notificationService.SendOTPSms(phoneVerificationModel.PhoneNumber, otpToken: otpToken);

            return new CustomHttpResponseData
            {
                HttpResponse = request.CreateResponse()
            };
        }

        public ValidationSummary ValidateKey(string key)
        {
            var errors = new List<string>();
            if (key != "e55cb4f7-5036-4fb9-b15b-102df960089f")
            {
                errors.Add("Invalid Test key");
            }

            return new ValidationSummary(!errors.Any(), errors.ToArray());
        }

        [Function(nameof(GetOTPToken))]
        public async Task<CustomHttpResponseData> GetOTPToken([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
        {
            var keyValidation = ValidateKey(request.GetQueryParameters()["key"]);
            if (!keyValidation.IsValid)
            {
                return await request.BuildValidationErrorResponseDataAsync(keyValidation);
            }

            if (request.GetQueryParameters()["email"] != null)
            {
                var emailVerificationModel = new EmailVerificationModel(request.GetQueryParameters()["email"]);
                var validation = emailVerificationModel.Validate();
                if (!validation.IsValid)
                {
                    return await request.BuildValidationErrorResponseDataAsync(validation);
                }

                var otpToken = await otpService.GenerateToken(emailVerificationModel.EmailAddress);

                return new CustomHttpResponseData { HttpResponse = await request.CreateObjectResponseAsync(new { OTPCode = otpToken }) };
            }
            else if (request.GetQueryParameters()["phone"] != null)
            {
                var phoneVerificationModel = new PhoneNumberVerificationModel(request.GetQueryParameters()["phone"]);

                var validation = phoneVerificationModel.Validate();
                if (!validation.IsValid)
                {
                    return await request.BuildValidationErrorResponseDataAsync(validation);
                }

                var otpToken = await otpService.GenerateToken(phoneVerificationModel.PhoneNumber);

                return new CustomHttpResponseData { HttpResponse = await request.CreateObjectResponseAsync(new { OTPCode = otpToken }) };
            }
            else
            {
                var errors = new List<string>();
                errors.Add("Invalid Request, an email address or phone number required to generate OTP");
                return await request.BuildValidationErrorResponseDataAsync(new ValidationSummary(!errors.Any(), errors.ToArray()));
            }

        }


        [Function(nameof(ValidateOTPToken))]
        public async Task<CustomHttpResponseData> ValidateOTPToken([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
        {
            var otpValidationModel = await request.ReadAsJsonAsync<OTPValidationModel>();

            if (!otpValidationModel.Validate().IsValid)
            {
                return new CustomHttpResponseData
                {
                    HttpResponse = request.CreateResponse(HttpStatusCode.BadRequest)
                };
            }

            if (featureOptions.DisableOtpValidation)
            {
                return new CustomHttpResponseData
                {
                    HttpResponse = request.CreateResponse(HttpStatusCode.OK)
                };
            }

            var isTokenValid = await otpService.ValidateToken(otpValidationModel.OTPToken, otpValidationModel.Data);

            if (!isTokenValid)
            {
                return new CustomHttpResponseData
                {
                    HttpResponse = request.CreateResponse(HttpStatusCode.BadRequest)
                };
            }

            return new CustomHttpResponseData
            {
                HttpResponse = request.CreateResponse(HttpStatusCode.OK)
            };
        }

    }
}
