using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using HSE.RP.API.Models;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Services
{
    public class DynamicsService
    {
        private readonly DynamicsOptions dynamicsOptions;
        private readonly SwaOptions swaOptions;
        public DynamicsService(IOptions<DynamicsOptions> dynamicsOptions, IOptions<SwaOptions> swaOptions)
        {
            this.dynamicsOptions = dynamicsOptions.Value;
            this.swaOptions = swaOptions.Value;
        }

        public async Task SendVerificationEmail(EmailVerificationModel emailVerificationModel, string otpToken)
        {
            await dynamicsOptions.EmailVerificationFlowUrl.PostJsonAsync(new
            {
                emailAddress = emailVerificationModel.EmailAddress,
                otp = otpToken,
                hrbRegUrl = swaOptions.Url
            });
        }

    }
}
