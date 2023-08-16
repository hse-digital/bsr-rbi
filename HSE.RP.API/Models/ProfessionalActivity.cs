using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models
{
    public record ProfessionalActivity
    {
        public ApplicantProfessionalBodyMembership ApplicantProfessionalBodyMembership { get; set; }
        public EmploymentType EmploymentType { get; set; }
    }
}
