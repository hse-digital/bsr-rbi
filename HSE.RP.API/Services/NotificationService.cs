using Flurl;
using Flurl.Http;
using HSE.RP.API.Models;
using HSE.RP.API.Models.Notification;
using HSE.RP.API.Models.OrdnanceSurvey;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HSE.RP.API.Services
{


    public class NotificationService
    {


        private readonly IntegrationsOptions integrationsOptions;
        private readonly SwaOptions swaOptions;
        public NotificationAPIKey notificationAPIKey;

        public NotificationService(IOptions<IntegrationsOptions> integrationsOptions, IOptions<SwaOptions> swaOptions)
        {
            this.integrationsOptions = integrationsOptions.Value;
            this.swaOptions = swaOptions.Value;
            this.notificationAPIKey = SplitAPIKey(this.integrationsOptions.NotificationServiceApiKey);

        }


        public static DateTime ConvertToGreenwichMeanTime(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        }

        public string GenerateSecurityToken(string apiKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(notificationAPIKey.apiSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("iss", apiKey)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }



        public NotificationAPIKey SplitAPIKey(string apiKey)
        {
            NotificationAPIKey notificationAPIKey = new NotificationAPIKey();
            string[] split = apiKey.Split('-');
            notificationAPIKey.apiName = split[0];
            notificationAPIKey.apiKey = String.Join("-", split.Skip(1).Take(5));
            notificationAPIKey.apiSecret = String.Join("-", split.Skip(6));
            return notificationAPIKey;

        }

        public async Task SendOTPEmail(string emailAddress, string otpToken)
        {

            string jwtToken = GenerateSecurityToken(notificationAPIKey.apiKey);

            var expirationTime = ConvertToGreenwichMeanTime(DateTime.UtcNow.AddHours(1));


            try
            {
                var response = await integrationsOptions.NotificationsAPIEndpoint
                .AppendPathSegments("v2", "notifications", "email")
                .WithHeaders(new
                {
                    Accept = "application/json",
                    Authorization = "Bearer " + jwtToken
                }
                )
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .AllowHttpStatus(HttpStatusCode.BadRequest)
                .AllowHttpStatus(HttpStatusCode.Unauthorized)
                .AllowHttpStatus(HttpStatusCode.TooManyRequests)
                .AllowHttpStatus(HttpStatusCode.InternalServerError)
                .PostJsonAsync(new EmailNotificationRequest
                {
                    email_address = emailAddress,
                    template_id = integrationsOptions.NotificationServiceOTPEmailTemplateId,
                    personalisation = new Dictionary<string, dynamic>()
                    {
                        { "security code", otpToken },
                        { "expiration_time", expirationTime.ToString("h:mmtt").ToLower() + " on " + expirationTime.ToString("%d MMMM yyyy")},
                        { "link", swaOptions.Url }
                    }
                }
                );
                if (response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    throw new Exception("Notification service not found");
                }
                if (response.StatusCode == (int)HttpStatusCode.Forbidden ||
                    response.StatusCode == (int)HttpStatusCode.BadRequest ||
                    response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                    response.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    var error = await response.GetJsonAsync<NotificationErrorResponse>();
                    throw new Exception(message: (error.errors[0].error + ": " + error.errors[0].message));
                }
                var result = await response.GetJsonAsync<EmailNotificationSuccessResponse>();

                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Notification service unknown error: " + ex.Message);
            }


        }

        public async Task SendOTPSms(string phone_number, string otpToken)
        {

            string jwtToken = GenerateSecurityToken(notificationAPIKey.apiKey);

            var expirationTime = ConvertToGreenwichMeanTime(DateTime.UtcNow.AddHours(1));


            try
            {
                var response = await integrationsOptions.NotificationsAPIEndpoint
                .AppendPathSegments("v2", "notifications", "sms")
                .WithHeaders(new
                {
                    Accept = "application/json",
                    Authorization = "Bearer " + jwtToken
                }
                )
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .AllowHttpStatus(HttpStatusCode.BadRequest)
                .AllowHttpStatus(HttpStatusCode.Unauthorized)
                .AllowHttpStatus(HttpStatusCode.TooManyRequests)
                .AllowHttpStatus(HttpStatusCode.InternalServerError)
                .PostJsonAsync(new SmsNotificationRequest
                {
                    phone_number = phone_number,
                    template_id = integrationsOptions.NotificationServiceOTPSmsTemplateId,
                    personalisation = new Dictionary<string, dynamic>()
                    {
                        { "security code", otpToken },
                        { "expiration_time", expirationTime.ToString("h:mmtt").ToLower() + " on " + expirationTime.ToString("%d MMMM yyyy")},
                    }
                }
                );
                if (response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    throw new Exception("Notification service not found");
                }
                if (response.StatusCode == (int)HttpStatusCode.Forbidden ||
                    response.StatusCode == (int)HttpStatusCode.BadRequest ||
                    response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                    response.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    var error = await response.GetJsonAsync<NotificationErrorResponse>();
                    throw new Exception(message: (error.errors[0].error + ": " + error.errors[0].message));
                }
                var result = await response.GetJsonAsync<SmsNotificationSuccessResponse>();

                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Notification service unknown error");
            }


        }


        public class NotificationAPIKey
        {
            public string apiName { get; set; }
            public string apiKey { get; set; }
            public string apiSecret { get; set; }
        }


    }




}

