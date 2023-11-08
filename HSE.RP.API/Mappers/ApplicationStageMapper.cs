using HSE.RP.API.Enums;
using HSE.RP.API.Models;
using HSE.RP.API.Models.Payment;
using HSE.RP.API.Models.Payment.Request;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace HSE.RP.API.Mappers
{
    public interface IApplicationStageMapper
    {
        BuildingProfessionApplicationStage? ToBuildingApplicationStage(ApplicationStage applicationStage);
    }
    public class ApplicationStageMapper : IApplicationStageMapper
    {

        public BuildingProfessionApplicationStage? ToBuildingApplicationStage(ApplicationStage applicationStage)
        {
            return applicationStage switch
            {
                ApplicationStage.PersonalDetails => BuildingProfessionApplicationStage.PersonalDetails,
                ApplicationStage.BuildingInspectorClass => BuildingProfessionApplicationStage.BuildingInspectorClass,
                ApplicationStage.Competency => BuildingProfessionApplicationStage.Competency,
                ApplicationStage.ProfessionalMembershipsAndEmployment => BuildingProfessionApplicationStage.ProfessionalMembershipsAndEmployment,
                ApplicationStage.ApplicationSummary => BuildingProfessionApplicationStage.ApplicationSummary,
                ApplicationStage.PayAndSubmit => BuildingProfessionApplicationStage.PayAndSubmit,
                ApplicationStage.ApplicationSubmitted => BuildingProfessionApplicationStage.ApplicationSubmitted,
                _ => null
            };
        }
    }
}
