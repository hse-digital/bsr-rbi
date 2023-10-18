namespace HSE.RP.API.Models
{
    public record ProfessionalActivity
    {
        public ApplicantProfessionalBodyMembership ApplicantProfessionalBodyMembership { get; set; }
        public ApplicantEmploymentDetails EmploymentDetails { get; set; }

    }
}
