using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Mail;
using Azure.Core;

namespace HSE.RP.API.Services
{

    public class NotificationService
    {
        private readonly IntegrationsOptions integrationsOptions;
        private readonly SwaOptions swaOptions;


        public NotificationService(IOptions<IntegrationsOptions> integrationsOptions, IOptions<SwaOptions> swaOptions)
        {
            this.integrationsOptions = integrationsOptions.Value;
            this.swaOptions = swaOptions.Value;
            
        }

        public async Task SendOTPEmail(string emailAddress, string otpToken)
        {

            var expirationTime = DateTime.UtcNow.Hour + 1;

            Dictionary<String, dynamic> personalisation = new Dictionary<String, dynamic>
                {
                    {"security_code", otpToken},
                    {"expiration_time", expirationTime.ToString()+":00"},
                    {"link", swaOptions.Url}
            };

            try
            {
                EmailNotificationResponse response = client.SendEmail(emailAddress: emailAddress, templateId: this.integrationsOptions.NotificationServiceOTPEmailTemplateId, personalisation: personalisation /*emailReplyToId: this.integrationsOptions.NotificationServiceReplyToId*/);
            }
            catch (NotifyClientException clientError)
            {
                Console.WriteLine(clientError.Message);
            }
            catch (NotifyAuthException authError)
            {
                Console.WriteLine(authError.Message);
            }
            }

        }



    }

