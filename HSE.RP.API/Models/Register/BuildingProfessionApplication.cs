using HSE.RP.API.Models.Enums;
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

        public required string Id { get; set; }

        public required string BuildingProfessionType { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string? DecisionCondition { get; set; }

        public required List<string> Countries { get; set; }

        public required DateTime CreationDate { get; set; }

        public required Applicant Applicant { get; set; }

        public Employer? Employer { get; set; }

        public List<string>? Classes { get; set; }

        public List<Activity>? Activities { get; set; }

    }
}
