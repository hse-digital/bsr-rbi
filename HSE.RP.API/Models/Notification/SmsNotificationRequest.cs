using System.Text.Json.Serialization;

namespace HSE.RP.API.Models.Notification
{
    internal class SmsNotificationRequest
    {
        [JsonPropertyName("phone_number")]
        public string phone_number { get; set; }

        [JsonPropertyName("template_id")]
        public string template_id { get; set; }

        [JsonPropertyName("personalisation")]
        public Dictionary<string, dynamic> personalisation { get; set; }


    }


}
