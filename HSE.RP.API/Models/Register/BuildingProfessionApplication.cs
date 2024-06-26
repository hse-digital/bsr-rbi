﻿using HSE.RP.API.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Register
{

    public interface IBuildingProfessionApplication
    {
        List<Activity> Activities { get; set; }
        Applicant Applicant { get; set; }
        string BuildingProfessionType { get; set; }
        List<string> Classes { get; set; }
        List<string> Countries { get; set; }
        DateTime CreationDate { get; set; }
        string DecisionCondition { get; set; }
        Employer Employer { get; set; }
        string Id { get; set; }
        DateTime ValidFrom { get; set; }
        DateTime ValidTo { get; set; }
    }

    public class BuildingProfessionApplication : IBuildingProfessionApplication
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("buildingProfessionType")]
        public required string BuildingProfessionType { get; set; }

        [JsonPropertyName("validFrom")]
        public DateTime ValidFrom { get; set; }

        [JsonPropertyName("validTo")]
        public DateTime ValidTo { get; set; }

        [JsonPropertyName("decisionCondition")]
        public string? DecisionCondition { get; set; }

        [JsonPropertyName("countries")]
        public required List<string> Countries { get; set; }

        [JsonPropertyName("creationDate")]
        public required DateTime CreationDate { get; set; }

        [JsonPropertyName("applicant")]
        public required Applicant Applicant { get; set; }

        [JsonPropertyName("employer")]
        public Employer? Employer { get; set; }

        [JsonPropertyName("classes")]
        public List<string>? Classes { get; set; }

        [JsonPropertyName("activities")]
        public List<Activity>? Activities { get; set; }

    }
}
