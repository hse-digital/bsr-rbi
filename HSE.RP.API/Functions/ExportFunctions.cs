﻿
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


        [Function(nameof(ExportRBIApplicationsToCosmos))]
        public async Task ExportRBIApplicationsToCosmos([TimerTrigger("0 5 0 * * *")] TimerInfo timer, [DurableClient] DurableTaskClient durableTaskClient)
        {

            await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(RunExportRBIOrchestration));


        }

        [Function(nameof(RunExportRBIOrchestration))]
        public async Task RunExportRBIOrchestration([OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var tasks = new List<Task>();
            var rbiApplications = await context.CallActivityAsync<List<DynamicsBuildingProfessionRegisterApplication>>(nameof(GetDynamicsRBIApplicationsToProcess));

            var existingApplications = await context.CallActivityAsync<List<string>>(nameof(GetExistingRegisterApplications));

            var applicationsToRemove = existingApplications.Except(rbiApplications.Select(x => x.ApplicationId)).ToList();

            var removes = applicationsToRemove.Select(async application => await context.CallActivityAsync(nameof(RemoveRBIApplication), application)).ToList();
            tasks.AddRange(removes);
            
            var imports = rbiApplications.Select(async application => await context.CallActivityAsync(nameof(ImportRBIApplication), application)).ToList();
            tasks.AddRange(imports);
            await Task.WhenAll(tasks);

        }

        [Function(nameof(GetDynamicsRBIApplicationsToProcess))]
        public async Task<List<DynamicsBuildingProfessionRegisterApplication>> GetDynamicsRBIApplicationsToProcess([ActivityTrigger] object x)
        {
            return await dynamicsService.GetDynamicsRBIApplicationsToProcess();
        }

        [Function(nameof(ImportRBIApplication))]
        [CosmosDBOutput("%Integrations:CosmosDatabase%", "%Integrations:CosmosContainer%", Connection = "CosmosConnection")]
        public async Task<BuildingProfessionApplication> ImportRBIApplication([ActivityTrigger] DynamicsBuildingProfessionRegisterApplication application)
        {

            var dynamicsApplication = await dynamicsService.GetDynamicsRBIApplicationData(application.BuildingProfessionApplicationDynamicsId);

            var applicationModel = applicationMapper.ToRBIApplication(dynamicsApplication);

            return applicationModel;
        }

        [Function(nameof(GetExistingRegisterApplications))]
        public async Task<List<string>> GetExistingRegisterApplications([ActivityTrigger] DynamicsBuildingProfessionRegisterApplication application)
        {

            var applicationIds = await cosmosDbService.GetIDsByBuildingProfessionType("BuildingInspector");

            return applicationIds;
        }

        [Function(nameof(RemoveRBIApplication))]
        public async Task RemoveRBIApplication([ActivityTrigger] string applicationId)
        {
           await cosmosDbService.RemoveItemAsync<BuildingProfessionApplication>(applicationId, "BuildingInspector");
        }



    }



}
