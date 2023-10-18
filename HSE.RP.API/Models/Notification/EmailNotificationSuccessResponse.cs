using System.Text.Json.Serialization;

namespace HSE.RP.API.Models.Notification
{
    public class EmailNotificationSuccessContent
    {
        [JsonPropertyName("subject")]
        public string subject { get; set; }

        [JsonPropertyName("body")]
        public string body { get; set; }

        [JsonPropertyName("from_email")]
        public string from_email { get; set; }
    }
    public class EmailNotificationSuccessResponse
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("reference")]
        public string reference { get; set; }

        [JsonPropertyName("content")]
        public EmailNotificationSuccessContent content { get; set; }

        [JsonPropertyName("uri")]
        public string uri { get; set; }

        [JsonPropertyName("template")]
        public EmailNotificationTemplate template { get; set; }
    }




    public class EmailNotificationTemplate
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("version")]
        public int? version { get; set; }

        [JsonPropertyName("uri")]
        public string uri { get; set; }
    }
}
