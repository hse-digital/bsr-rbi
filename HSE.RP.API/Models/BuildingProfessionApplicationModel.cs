using Google.Protobuf.WellKnownTypes;
using HSE.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models
{
    public record BuildingProfessionApplicationModel
    (
        [property: JsonPropertyName("id")] string Id,
        PersonalDetails PersonalDetails = null,
        BuildingInspectorClass BuildingInspectorClass = null,
        ApplicationStatus ApplicationStatus = ApplicationStatus.None) : IValidatableModel
    {
        public ValidationSummary Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(PersonalDetails.ApplicantEmail.Email))
            {
                errors.Add("Applicant email address is required");
            }
            if (string.IsNullOrWhiteSpace(PersonalDetails.ApplicantPhone.PhoneNumber))
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
    public enum ComponentCompletionState
    {
        NotStarted = 0,
        InProgress = 1,
        Complete = 2
    }

    public record ApplicantName
    (
        string FirstName = null,
        string LastName = null,
        ComponentCompletionState IsComplete = ComponentCompletionState.NotStarted
    );

    public record ApplicantPhone
    (
        string PhoneNumber = null,
        ComponentCompletionState IsComplete = ComponentCompletionState.NotStarted
    );

    public record ApplicantEmail
    (
        string Email = null,
        ComponentCompletionState IsComplete = ComponentCompletionState.NotStarted
    );

    public record ApplicantNationalInsuranceNumber
    (
        string NationalInsuranceNumber = null,
        ComponentCompletionState IsComplete = ComponentCompletionState.NotStarted
    );

    public record PersonalDetails
    (
        ApplicantName ApplicantName = null,
        string ApplicantPhoto = null,
        BuildingAddress ApplicantAddress = null,
        ApplicantPhone ApplicantPhone = null,
        ApplicantPhone ApplicantAlternativePhone = null,
        ApplicantEmail ApplicantEmail = null,
        ApplicantEmail ApplicantAlternativeEmail = null,
        string ApplicantProofOfIdentity = null,
        ApplicantNationalInsuranceNumber ApplicantNationalInsuranceNumber = null
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
        public bool? IsManual { get; init; }
        public string ClassificationCode { get; init; }
        public ComponentCompletionState IsComplete {get; init;} 
    }

    public record BuildingInspectorClass
    (
        ClassSelection ClassSelection = null,
        string ClassDetails = null,
        string Country = null
    );

    public record ClassSelection
    (
        string ClassType = null,
        ComponentCompletionState IsComplete = ComponentCompletionState.NotStarted
    );

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
