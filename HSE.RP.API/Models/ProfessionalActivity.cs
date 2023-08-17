using System;

namespace HSE.RP.API.Models
{
    public record ProfessionalActivity
    {
        public ApplicantProfessionalBodyMembership ApplicantProfessionalBodyMembership { get; set; }
        public EmploymentTypeSelection EmploymentTypeSelection { get; set; }
    }
}
