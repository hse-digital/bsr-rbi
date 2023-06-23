using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Notification
{
    public class NotificationError
    {
        [JsonPropertyName("error")]
        public string error { get; set; }

        [JsonPropertyName("message")]
        public string message { get; set; }
    }

    public class NotificationErrorResponse
    {
        [JsonPropertyName("status_code")]
        public int? status_code { get; set; }

        [JsonPropertyName("errors")]
        public List<NotificationError> errors { get; set; }
    }
}
