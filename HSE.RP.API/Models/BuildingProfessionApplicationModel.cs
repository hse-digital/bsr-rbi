using HSE.API.Models;
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
    (
     string FirstName = null,
     string LastName = null
    );


    public record PersonalDetails
    (
        ApplicantName ApplicantName = null,
        ApplicantDateOfBirth ApplicantDateOfBirth = null,
        BuildingAddress ApplicantAddress = null,
        string ApplicantPhone = null,
        string ApplicantAlternativePhone = null,
        string ApplicantEmail = null,
        string ApplicantAlternativeEmail = null,
        string ApplicantProofOfIdentity = null,
        string ApplicantNationalInsuranceNumber = null
    );

    public record ApplicantDateOfBirth
    {
        string Day = null;
        string Month = null;
        string Year = null;
    }

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
