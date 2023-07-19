﻿
using HSE.RP.Domain.Entities;
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
        Dictionary<string, StageCompletionState> StageStatus = null,
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

    public enum StageCompletionState
    {
        NotStarted = 0,
        InProgress = 1,
        Complete = 2
    }

    public record ApplicantName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    }

    public record ApplicantPhone
    { 
        public string PhoneNumber { get; set; }
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    }
    public record ApplicantDateOfBirth
    {
        public string Day  { get; set; }
        public string Month  { get; set; }
        public string Year  { get; set; }
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;

    }

    public record ApplicantEmail
    {
        public string Email { get; set; }

        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    };

    public record ApplicantNationalInsuranceNumber
    {
        public string NationalInsuranceNumber { get; set; }
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    };

    public record PersonalDetails
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
