using System;
using HSE.RP.Domain.Entities;

namespace HSE.RP.API.Models
{
    public record PersonalDetails
    {
        public ApplicantName ApplicantName { get; set; }
        public AddressModel ApplicantAddress { get; set; }
        public ApplicantPhone ApplicantPhone { get; set; }
        public ApplicantPhone ApplicantAlternativePhone { get; set; }
        public ApplicantEmail ApplicantEmail { get; set; }
        public ApplicantEmail ApplicantAlternativeEmail { get; set; }
        public ApplicantDateOfBirth ApplicantDateOfBirth { get; set; }
        public ApplicantNationalInsuranceNumber ApplicantNationalInsuranceNumber { get; set; }

    }
}

