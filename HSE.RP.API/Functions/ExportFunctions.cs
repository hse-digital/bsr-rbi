
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

namespace HSE.RP.API.Functions
{
    public interface IExportFunctions
    {
        Task ExportRBIApplicationsToCosmos([TimerTrigger("0 5 0 * * *")] TimerInfo timer);

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
        public async Task ExportRBIApplicationsToCosmos([TimerTrigger("0 5 0 * * *")] TimerInfo timer)
        {

            // Remove old RBI applications from Cosmos DB
            await cosmosDbService.RemoveItemsByBuildingProfessionTypeAndCreationDateAsync<BuildingProfessionApplication>("BuildingInspector", DateTime.Now.Date);

            // Get RBI applications from Dynamics
            var DynamicsRBIApplications = await dynamicsService.GetDynamicsRBIApplications();

            List<BuildingProfessionApplication> RBIApplications = new List<BuildingProfessionApplication>();

            // Map Dynamics RBI applications to RBIApplication model
            foreach (DynamicsBuildingProfessionRegisterApplication dynamicsRBIApplication in DynamicsRBIApplications)
            {
                RBIApplications.Add(applicationMapper.ToRBIApplication(dynamicsRBIApplication));
            }

            // Add RBI applications to Cosmos DB
            foreach (BuildingProfessionApplication rbiApplication in RBIApplications)
            {
                await cosmosDbService.AddItemAsync(rbiApplication, rbiApplication.BuildingProfessionType);
            }

        }

    }



}
