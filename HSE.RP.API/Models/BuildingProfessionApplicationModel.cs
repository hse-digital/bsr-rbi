using HSE.RP.API.Models;
using HSE.RP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models
{
    public record BuildingProfessionApplicationModel
    (
        [property: JsonPropertyName("id")] string Id,
        PersonalDetails PersonalDetails = null,
        ApplicationStatus ApplicationStatus = ApplicationStatus.None) : IValidatableModel
    {
        public ValidationSummary Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(PersonalDetails.ApplicantEmail))
            {
                errors.Add("Applicant email address is required");
            }
            if (string.IsNullOrWhiteSpace(PersonalDetails.ApplicantPhone))
            {
                errors.Add("Applicant phone number is required");
            }
            if (string.IsNullOrWhiteSpace(PersonalDetails.ApplicantName.FirstName))
            {
                errors.Add("Applicant first name is required");
            }
            if (string.IsNullOrWhiteSpace(PersonalDetails.ApplicantName.LastName))
            {
                errors.Add("Applicant last name is required");
            }


            return new ValidationSummary(!errors.Any(), errors.ToArray());
        }
    }

    public record ApplicantName
    {
        public string FirstName { get; set; }
        public  string LastName { get; set; }
    };

    public record ApplicantDateOfBirth
    {
        public string Day  { get; set; }
        public string Month  { get; set; }
        public string Year  { get; set; }
    }


    public record PersonalDetails
    {
        public ApplicantName ApplicantName { get; set; }
        public ApplicantDateOfBirth ApplicantDateOfBirth { get; set; }
        public BuildingAddress ApplicantAddress { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantAlternativePhone { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantAlternativeEmail { get; set; }
        public string ApplicantNationalInsuranceNumber { get; set; }
    };


    [Flags]
    public enum ApplicationStatus
    {
        None = 0,
        EmailVerified = 1,
        PhoneVerified = 2,
        PersonalDetailsComplete = 4,
        BuildingInspectorClassComplete = 8,
        CompetencyComplete = 16,
        ProfessionalActivityComplete = 32,
        ApplicationSubmissionComplete = 64,
        PaymentInProgress = 128,
        PaymentComplete = 256,
    }

    public record Submit();




}
