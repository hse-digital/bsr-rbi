using System.Text.Json.Serialization;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record BuildingProfessionApplicationModel
    (
        [property: JsonPropertyName("id")] string Id,
        PersonalDetails PersonalDetails = null,
        BuildingInspectorClass InspectorClass = null,
        Competency Competency = null,
        ApplicantProfessionBodyMemberships ProfessionalMemberships = null,
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

}
