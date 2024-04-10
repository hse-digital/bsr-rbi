
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
using Microsoft.Extensions.Logging;
using Azure.Core;

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
        private readonly ILogger<ExportFunctions> logger;

        public ExportFunctions(IDynamicsService dynamicsService, IApplicationMapper applicationMapper, ICosmosDbService cosmosDbService, ILogger<ExportFunctions> logger)
        {
            this.dynamicsService = dynamicsService;
            this.applicationMapper = applicationMapper;
            this.cosmosDbService = cosmosDbService;
            this.logger = logger;
        }

        [Function(nameof(UpdateRBIApplicationsFromDynamics))]
        public async Task UpdateRBIApplicationsFromDynamics([TimerTrigger("0 5 0 * * *")] TimerInfo timer, [DurableClient] DurableTaskClient durableTaskClient)
        {

            await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(RunUpdateRBIOrchestration));

        }

        [Function(nameof(ExportRBIApplicationsToCosmos))]
        public async Task<HttpResponseData> ExportRBIApplicationsToCosmos([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
        {

            await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(RunExportRBIOrchestration));

            return request.CreateResponse();

        }

        [Function(nameof(RunUpdateRBIOrchestration))]
        public async Task RunUpdateRBIOrchestration([OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var tasks = new List<Task>();

            var rbiApplications = await context.CallActivityAsync<List<DynamicsBuildingProfessionRegisterApplication>>(nameof(GetDynamicsRBIApplicationsToProcess));
            var existingApplications = await context.CallActivityAsync<List<string>>(nameof(GetExistingRegisterApplications));
            var applicationsToRemove = existingApplications.Except(rbiApplications.Select(x => x.ApplicationId)).ToList();


            var removes = applicationsToRemove.Select(async application => await context.CallActivityAsync(nameof(RemoveRBIApplication), application)).ToList();
            tasks.AddRange(removes);

            var applicationsToUpdate = await context.CallActivityAsync<List<string>>(nameof(GetModifiedRegisterApplications));

            var imports = applicationsToUpdate.Select(async application => await context.CallActivityAsync(nameof(UpdateRBIApplication), application)).ToList();


            tasks.AddRange(imports);

            await Task.WhenAll(tasks);
            logger.LogInformation("Applications removed:");
            foreach (var application in applicationsToRemove)
            {
                logger.LogInformation(application);
            }

            logger.LogInformation("Applications updated:");
            foreach (var application in applicationsToUpdate)
            {
                logger.LogInformation(application);
            }



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

            logger.LogInformation($"Applications removed: {applicationsToRemove.Count}");
            logger.LogInformation($"Applications updated: {rbiApplications.Count}");
        }

        [Function(nameof(GetModifiedRegisterApplications))]
        public async Task<List<string>> GetModifiedRegisterApplications([ActivityTrigger] DynamicsBuildingProfessionRegisterApplication application)
        {



            var modifiedApplications = await dynamicsService.GetDynamicsModifiedRBIApplicationData();
            var modifiedEmployment = await dynamicsService.GetDynamicsModifiedRBIApplicationEmploymentDetails();
            var modifiedCountry = await dynamicsService.GetDynamicsModifiedRBIApplicationCountryDetails();
            var modifiedClasses = await dynamicsService.GetDynamicsModifiedRBIApplicationClassDetails();
            var modifiedActivities = await dynamicsService.GetDynamicsModifiedRBIApplicationActivityDetails();

            var applicationIds = modifiedApplications.Select(x => x.BuildingProfessionApplicationDynamicsId).ToList();
            applicationIds.AddRange(modifiedEmployment.Select(x => x.applicationId));
            applicationIds.AddRange(modifiedCountry.Select(x => x.applicationId));
            applicationIds.AddRange(modifiedClasses.Select(x => x.applicationId));
            applicationIds.AddRange(modifiedActivities.Select(x => x.applicationId));

            return applicationIds.Distinct().ToList();
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

        [Function(nameof(UpdateRBIApplication))]
        [CosmosDBOutput("%Integrations:CosmosDatabase%", "%Integrations:CosmosContainer%", Connection = "CosmosConnection")]
        public async Task<BuildingProfessionApplication> UpdateRBIApplication([ActivityTrigger] string applicationId)
        {

            var dynamicsApplication = await dynamicsService.GetDynamicsRBIApplicationData(applicationId);

            var applicationModel = applicationMapper.ToRBIApplication(dynamicsApplication);

            logger.LogInformation($"Updating application {applicationModel.Id}: {applicationModel.Applicant.ApplicantName ?? "None"} in register");

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
           logger.LogInformation($"Removing application {applicationId} from register");
           await cosmosDbService.RemoveItemAsync<BuildingProfessionApplication>(applicationId, "BuildingInspector");
        }



    }



}
