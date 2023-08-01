using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using OtpNet;

namespace HSE.RP.API.Services
{
    public class OTPService
    {
        public record TokenRequestModel(string? Token = null, string? TokenData = null);

        public record TokenResponseModel(string? Token, string? TokenData, DateTime? Expiry, string? id = null);


        private readonly IntegrationsOptions integrationOptions;
        public OTPService(IOptions<IntegrationsOptions> integrationOptions)
        {
            this.integrationOptions = integrationOptions.Value;
        }
        public virtual async Task<string> GenerateToken(string tokenPersonaIdentifier)
        {
            var response = await integrationOptions.CommonAPIEndpoint
                .AppendPathSegment("api")
                .AppendPathSegment("GenerateToken")
                .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
                .AllowHttpStatus(HttpStatusCode.BadRequest)
                .PostJsonAsync(new TokenRequestModel { TokenData=tokenPersonaIdentifier });

            var responseModel = await response.GetJsonAsync<TokenResponseModel>();
            return responseModel.Token;
        }

        public virtual async Task<bool> ValidateToken(string otpToken, string tokenPersonaIdentifier)
        {
            var response = await integrationOptions.CommonAPIEndpoint
                .AppendPathSegment("api")
                .AppendPathSegment("ValidateToken")
                .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
                .AllowHttpStatus(HttpStatusCode.BadRequest)
                .PostJsonAsync(new TokenRequestModel { Token = otpToken, TokenData = tokenPersonaIdentifier });

            return response.StatusCode == (int)HttpStatusCode.OK;
        }
        public virtual async Task<bool> CheckIsTokenExpired(string otpToken, string tokenPersonaIdentifier)
        {
            var response = await integrationOptions.CommonAPIEndpoint
                .AppendPathSegment("api")
                .AppendPathSegment("CheckIsTokenExpired")
                .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
                .AllowHttpStatus(HttpStatusCode.BadRequest)
                .PostJsonAsync(new TokenRequestModel { Token = otpToken, TokenData = tokenPersonaIdentifier });
            if(response.StatusCode == (int)HttpStatusCode.ExpectationFailed)
            {
                return true;
            }

            return false;
        }
    }
}
