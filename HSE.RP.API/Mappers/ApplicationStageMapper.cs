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
            if (applicationStage == ApplicationStage.PersonalDetails)
            {
                return BuildingProfessionApplicationStage.PersonalDetails;
            }
            else if (applicationStage == ApplicationStage.BuildingInspectorClass)
            {
                return BuildingProfessionApplicationStage.BuildingInspectorClass;
            }
            else if (applicationStage == ApplicationStage.Competency)
            {
                return BuildingProfessionApplicationStage.Competency;
            }
            else if (applicationStage == ApplicationStage.ProfessionalMembershipsAndEmployment)
            {
                return BuildingProfessionApplicationStage.ProfessionalMembershipsAndEmployment;
            }
            else if (applicationStage == ApplicationStage.ApplicationSummary)
            {
                return BuildingProfessionApplicationStage.ApplicationSummary;
            }
            else if (applicationStage == ApplicationStage.PayAndSubmit)
            {
                return BuildingProfessionApplicationStage.PayAndSubmit;
            }
            else if (applicationStage == ApplicationStage.ApplicationSubmitted)
            {
                return BuildingProfessionApplicationStage.ApplicationSubmitted;
            }
            else
            {
                return null;
            }
        }
    }
}
