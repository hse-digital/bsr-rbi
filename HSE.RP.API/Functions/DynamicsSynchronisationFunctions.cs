using System.Globalization;
using AutoMapper;
using Flurl;
using Flurl.Http;
using HSE.RP.API.Extensions;
using HSE.RP.API.Models;
using HSE.RP.API.Models.DynamicsSynchronisation;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Options;
using BuildingApplicationStatus = HSE.RP.Domain.Entities.BuildingApplicationStatus;

namespace HSE.RP.API.Functions;

public class DynamicsSynchronisationFunctions
{
    private readonly DynamicsService dynamicsService;
    private readonly IMapper mapper;
    private readonly IntegrationsOptions integrationOptions;

    public DynamicsSynchronisationFunctions(DynamicsService dynamicsService, IOptions<IntegrationsOptions> integrationOptions, IMapper mapper)
    {
        this.dynamicsService = dynamicsService;
        this.mapper = mapper;
        this.integrationOptions = integrationOptions.Value;
    }


    [Function(nameof(SyncDeclaration))]
    public async Task<HttpResponseData> SyncDeclaration([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, [DurableClient] DurableTaskClient durableTaskClient)
    {
        var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseDeclaration), buildingProfessionApplicationModel);

        return request.CreateResponse();
    }

    [Function(nameof(SyncPayment))]
    public async Task<HttpResponseData> SyncPayment([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, [DurableClient] DurableTaskClient durableTaskClient)
    {
        var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchronisePayment), buildingProfessionApplicationModel);

        return request.CreateResponse();
    }


    [Function(nameof(SynchroniseDeclaration))]
    public async Task SynchroniseDeclaration([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);
        if (dynamicsBuildingApplication != null)
        {
            await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplication), new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingApplication));
        }
    }

    [Function(nameof(SynchronisePayment))]
    public async Task SynchronisePayment([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);
        if (dynamicsBuildingProfessionApplication != null)
        {
            var buildingProfessionApplicationWrapper = new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingProfessionApplication);
            await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplication), buildingProfessionApplicationWrapper);

            var payments = await orchestrationContext.CallActivityAsync<List<DynamicsPayment>>(nameof(GetDynamicsPayments), buildingProfessionApplicationModel.Id);
            var paymentSyncTasks = payments.Select(async payment =>
            {
                var paymentResponse = await orchestrationContext.CallActivityAsync<PaymentResponseModel>(nameof(GetPaymentStatus), payment.bsr_govukpaymentid);
                if (paymentResponse != null)
                {
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdatePayment), new BuildingProfessionApplicationPayment(dynamicsBuildingProfessionApplication.bsr_buildingproappid, paymentResponse));
                    if (paymentResponse.Status == "success"/* && dynamicsBuildingProfessionApplication.bsr_applicationstage != BuildingApplicationStage.ApplicationSubmitted*/)
                    {
                        await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingToSubmitted), dynamicsBuildingProfessionApplication);
                    }
                }

                return paymentResponse;
            }).ToArray();

            await Task.WhenAll(paymentSyncTasks);
        }
    }

    [Function(nameof(GetBuildingProfessionApplicationUsingId))]
    public Task<DynamicsBuildingProfessionApplication> GetBuildingProfessionApplicationUsingId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetBuildingProfessionApplicationUsingId(applicationId);
    }

    [Function(nameof(UpdateBuildingProfessionApplication))]
    public Task UpdateBuildingProfessionApplication([ActivityTrigger] BuildingProfessionApplicationWrapper buildingProfessionApplicationWrapper)
    {
        //var stage = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_applicationstage == BuildingApplicationStage.ApplicationSubmitted ? BuildingApplicationStage.ApplicationSubmitted : buildingProfessionApplicationWrapper.Stage;
        return dynamicsService.UpdateBuildingProfessionApplication(buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication, new DynamicsBuildingProfessionApplication
        {
            //bsr_applicationstage = stage,
            //bsr_declarationconfirmed = buildingProfessionApplicationWrapper.Stage is BuildingApplicationStage.ApplicationSubmitted or BuildingApplicationStage.PayAndApply,
        });
    }

    [Function(nameof(UpdateBuildingToSubmitted))]
    public Task UpdateBuildingToSubmitted([ActivityTrigger] DynamicsBuildingProfessionApplication buildingProfessionApplication)
    {
        return dynamicsService.UpdateBuildingProfessionApplication(buildingProfessionApplication, new DynamicsBuildingProfessionApplication
        {
/*            bsr_submittedon = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            bsr_applicationstage = BuildingApplicationStage.ApplicationSubmitted*/
        });
    }



    [Function(nameof(GetDynamicsPayments))]
    public Task<List<DynamicsPayment>> GetDynamicsPayments([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetPayments(applicationId);
    }

    [Function(nameof(CreateOrUpdatePayment))]
    public async Task CreateOrUpdatePayment([ActivityTrigger] BuildingProfessionApplicationPayment buildingProfessionApplicationPayment)
    {
        await dynamicsService.CreatePayment(buildingProfessionApplicationPayment);
    }

    [Function(nameof(GetPaymentStatus))]
    public async Task<PaymentResponseModel> GetPaymentStatus([ActivityTrigger] string paymentId)
    {
        PaymentApiResponseModel response;
        var retryCount = 0;

        do
        {
            response = await integrationOptions.PaymentEndpoint
                .AppendPathSegments("v1", "payments", paymentId)
                .WithOAuthBearerToken(integrationOptions.PaymentApiKey)
                .GetJsonAsync<PaymentApiResponseModel>();

            if (ShouldRetry(response.state.status, retryCount))
            {
                await Task.Delay(3000);
                retryCount++;
            }
        } while (ShouldRetry(response.state.status, retryCount));

        return mapper.Map<PaymentResponseModel>(response);
    }

    [Function(nameof(UpdateBuildingProfessionApplicationInCosmos))]
    [CosmosDBOutput("hseportal", "regulated_building_professions", Connection = "CosmosConnection")]
    public BuildingProfessionApplicationModel UpdateBuildingProfessionApplicationInCosmos([ActivityTrigger] BuildingProfessionApplicationModel buildingProfessionApplicationModel)
    {
        return buildingProfessionApplicationModel;
    }

    private bool ShouldRetry(string status, int retryCount)
    {
        if (status != "success" && status != "failed" && status != "cancelled" && status != "error")
        {
            return retryCount < 3;
        }

        return false;
    }
}