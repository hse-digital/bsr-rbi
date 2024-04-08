using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Register
{
    public class Applicant
    {
        [JsonPropertyName("applicantName")]
        public required string ApplicantName { get; set; }
    }
}
