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
        BuildingInspectorClass InspectorClass = null,
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
    {
        public BuildingInspectorClassType? Class { get; set; }
        public BuildingInspectorRegulatedActivies? Activities { get; set; }
        public BuildingAssessingPlansCategories? Categories { get; set; }
    }
        
    public enum BuildingInspectorClassType
    {
        ClassNone = 0,
        Class1 = 1,
        Class2 = 2,
        Class3 = 3
    }

    public record BuildingInspectorRegulatedActivies
    {
        public bool? AssessingPlans { get; set; }
        public bool? Inspection {get;set; }
        public ComponentCompletionState CompletionState { get; set; }

    }

    public record BuildingAssessingPlansCategories
    {
        public bool? CategoryA { get; set; }
        public bool? CategoryB { get; set; }
        public bool? CategoryC { get; set; }
        public bool? CategoryD { get; set; }
        public bool? CategoryE { get; set; }
        public bool? CategoryF { get; set; }
        public ComponentCompletionState CompletionState { get; set; }
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
