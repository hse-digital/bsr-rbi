﻿using System;
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
        string ApplicantAddress = null,
        string ApplicantPhone = null,
        string ApplicantAlternativePhone = null,
        string ApplicantEmail = null,
        string ApplicantAlternativeEmail = null,
        string ApplicantProofOfIdentity = null
    );

    [Flags]
    public enum ApplicationStatus
    {
        None = 0,
        PersonalDetailsComplete = 1,
        BuildingInspectorClassComplete = 2,
        CompetencyComplete = 4,
        ProfessionalActivityComplete = 8,
        ApplicationOverviewComplete = 16,
        PayAndSumbitComplete = 32,
    }

    public record Submit();




}
