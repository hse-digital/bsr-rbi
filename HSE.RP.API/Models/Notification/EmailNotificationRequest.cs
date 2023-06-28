using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Notification
{
    internal class EmailNotificationRequest
    {
        [JsonPropertyName("email_address")]
        public string email_address { get; set; }

        [JsonPropertyName("template_id")]
        public string template_id { get; set; }

        [JsonPropertyName("personalisation")]
        public Dictionary<string, dynamic> personalisation { get; set; }


    }


}
