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
        string ApplicantPhoto = null,
        BuildingAddress ApplicantAddress = null,
        string ApplicantPhone = null,
        string ApplicantAlternativePhone = null,
        string ApplicantEmail = null,
        string ApplicantAlternativeEmail = null,
        string ApplicantProofOfIdentity = null,
        string ApplicantNationalInsuranceNumber = null
    );

    public record BuildingAddress
    {
        public string UPRN { get; init; }
        public string USRN { get; init; }
        public string Address { get; init; }
        public string AddressLineTwo { get; init; }
        public string BuildingName { get; init; }
        public string Number { get; init; }
        public string Street { get; init; }
        public string Town { get; init; }
        public string Country { get; init; }
        public string AdministrativeArea { get; init; }
        public string Postcode { get; init; }
        public bool IsManual { get; init; }
        public string ClassificationCode { get; init; }
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
        PayAndSubmitComplete = 128,
    }

    public record Submit();




}
