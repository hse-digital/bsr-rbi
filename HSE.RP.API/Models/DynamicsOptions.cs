using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models
{
    public class DynamicsOptions
    {
        public const string Dynamics = "Dynamics";
        public string EnvironmentUrl { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string EmailVerificationFlowUrl { get; set; }
        public string LocalAuthorityTypeId { get; set; }
    }
}
