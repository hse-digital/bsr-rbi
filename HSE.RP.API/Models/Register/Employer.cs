using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Register
{
    public class Employer
    {
        [JsonPropertyName("employerName")]
        public string? EmployerName { get; set; }

        [JsonPropertyName("employerAddress")]
        public string? EmployerAddress { get; set; }

        [JsonPropertyName("employmentType")]
        public string? EmploymentType { get; set; }

    }
}
