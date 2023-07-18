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
        public ApplicantName ApplicantName { get; set; }
        public BuildingAddress ApplicantAddress { get; set; }
        public ApplicantPhone ApplicantPhone { get; set; }
        public ApplicantPhone ApplicantAlternativePhone { get; set; }
        public ApplicantEmail ApplicantEmail { get; set; }
        public ApplicantEmail ApplicantAlternativeEmail { get; set; }
        public ApplicantDateOfBirth ApplicantDateOfBirth { get; set; }
        public ApplicantNationalInsuranceNumber ApplicantNationalInsuranceNumber { get; set; }
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
