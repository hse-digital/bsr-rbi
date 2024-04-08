
using HSE.RP.API.Services;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE.RP.API.Models.Register;
using HSE.RP.API.Models.DynamicsDataExport;
using HSE.RP.API.Mappers;
using HSE.RP.API.Extensions;
using System.Net;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;

namespace HSE.RP.API.Functions
{
    public interface IExportFunctions
    {
      

    }

    public class ExportFunctions : IExportFunctions
    {
        private readonly IDynamicsService dynamicsService;
        private readonly IApplicationMapper applicationMapper;
        private readonly ICosmosDbService cosmosDbService;

        public ExportFunctions(IDynamicsService dynamicsService, IApplicationMapper applicationMapper, ICosmosDbService cosmosDbService)
        {
            this.dynamicsService = dynamicsService;
            this.applicationMapper = applicationMapper;
            this.cosmosDbService = cosmosDbService;
        }

        // Export RBI applications to Cosmos DB
        [Function(nameof(ExportRBIApplicationsToCosmos))]
        public async Task ExportRBIApplicationsToCosmos([TimerTrigger("0 5 0 * * *")] TimerInfo timer, [DurableClient] DurableTaskClient durableTaskClient)
        {

            await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(RunExportRBIOrchestration));


        }

        [Function(nameof(RunExportRBIOrchestration))]
        public async Task RunExportRBIOrchestration([OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var rbiApplications = await context.CallActivityAsync<List<DynamicsBuildingProfessionRegisterApplication>>(nameof(GetDynamicsRBIApplications));

            var imports = rbiApplications.Select(async application => await context.CallActivityAsync(nameof(ImportRBIApplication), application)).ToList();
            await Task.WhenAll(imports);
        }

        [Function(nameof(GetDynamicsRBIApplications))]
        public async Task<List<DynamicsBuildingProfessionRegisterApplication>> GetDynamicsRBIApplications([ActivityTrigger] object x)
        {
            return await dynamicsService.GetDynamicsRBIApplications();
        }

        [Function(nameof(ImportRBIApplication))]
        [CosmosDBOutput("%Integrations:CosmosDatabase%", "%Integrations:CosmosContainer%", Connection = "CosmosConnection")]
        public async Task<BuildingProfessionApplication> ImportRBIApplication([ActivityTrigger] DynamicsBuildingProfessionRegisterApplication application)
        {


            var employmentDetails = await dynamicsService.GetDynamicsRBIApplicationEmploymentDetails(application.BuildingProfessionApplicationDynamicsId);
            application.ApplicantEmploymentDetails = employmentDetails;

            
            var classDetails = await dynamicsService.GetDynamicsRBIApplicationClassDetails(application.BuildingProfessionApplicationDynamicsId);
            application.ApplicantClassDetails = classDetails;

            var activityDetails = await dynamicsService.GetDynamicsRBIApplicationActivityDetails(application.BuildingProfessionApplicationDynamicsId);
            application.ApplicantActivityDetails = activityDetails;

            var countryDetails = await dynamicsService.GetDynamicsRBIApplicationCountryDetails(application.BuildingProfessionApplicationDynamicsId);
            application.ApplicantCountryDetails = countryDetails;

            var applicationModel = applicationMapper.ToRBIApplication(application);

            return applicationModel;
        }



    }



}
