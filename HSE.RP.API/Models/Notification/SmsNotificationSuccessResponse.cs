using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Notification
{
    public class SmsNotificationSuccessContent
    {
        [JsonPropertyName("body")]
        public string body { get; set; }

        [JsonPropertyName("from_number")]
        public string from_number { get; set; }
    }

    public class SmsNotificationSuccessResponse
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("reference")]
        public string reference { get; set; }

        [JsonPropertyName("content")]
        public SmsNotificationSuccessContent content { get; set; }

        [JsonPropertyName("uri")]
        public string uri { get; set; }

        [JsonPropertyName("template")]
        public SmsNotificationSuccessTemplate template { get; set; }
    }

    public class SmsNotificationSuccessTemplate
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("version")]
        public int? version { get; set; }

        [JsonPropertyName("uri")]
        public string uri { get; set; }
    }

}
