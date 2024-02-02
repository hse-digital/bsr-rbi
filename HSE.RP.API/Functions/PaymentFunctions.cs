using AutoMapper;
using HSE.RP.API.Enums;
using HSE.RP.API.Extensions;
using HSE.RP.API.Mappers;
using HSE.RP.API.Models;
using HSE.RP.API.Models.DynamicsSynchronisation;
using HSE.RP.API.Models.Payment;
using HSE.RP.API.Models.Payment.Request;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;

namespace HSE.RP.API.Functions;

public class PaymentFunctions
{
    private readonly IDynamicsService dynamicsService;
    private readonly IMapper mapper;
    private readonly IntegrationsOptions integrationOptions;
    private readonly SwaOptions swaOptions;
    private readonly IPaymentService paymentService;
    private readonly IPaymentMapper paymentMapper;


    public PaymentFunctions(IOptions<IntegrationsOptions> integrationOptions, IOptions<SwaOptions> swaOptions, IDynamicsService dynamicsService, IMapper mapper, IPaymentService paymentService
            , IPaymentMapper paymentMapper)
    {
        this.dynamicsService = dynamicsService;
        this.mapper = mapper;
        this.integrationOptions = integrationOptions.Value;
        this.swaOptions = swaOptions.Value;
        this.paymentService = paymentService;
        this.paymentMapper = paymentMapper;
    }

    [Function(nameof(InitialisePayment))]
    public async Task<HttpResponseData> InitialisePayment(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{nameof(InitialisePayment)}/{{applicationId}}")]
    HttpRequestData request,
        [CosmosDBInput("hseportal", "regulated_building_professions", Id = "{applicationId}", PartitionKey = "{applicationId}", Connection = "CosmosConnection")]
        BuildingProfessionApplicationModel applicationModel)
    {
        var dynamicsApplication = await dynamicsService.GetBuildingProfessionApplicationUsingId(applicationModel.Id);
        var newPayment = paymentService.BuildPaymentRequest(applicationModel);
        var paymentResponse = await paymentService.CreateCardPayment(newPayment);
        paymentResponse.ApplicationId = dynamicsApplication.bsr_buildingprofessionapplicationid;
        var dynamicsPayment = await dynamicsService.CreatePaymentAsync(paymentMapper.ToDynamics(paymentResponse), dynamicsApplication.bsr_buildingprofessionapplicationid);
        paymentResponse.bsr_paymentId = dynamicsPayment.bsr_paymentid;
        return await request.CreateObjectResponseAsync(paymentResponse);
    }

    [Function(nameof(InitialiseInvoicePayment))]
    public async Task<HttpResponseData> InitialiseInvoicePayment(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{nameof(InitialiseInvoicePayment)}/{{applicationId}}")]
    HttpRequestData request, EncodedRequest encodedRequest,
        [CosmosDBInput("hseportal", "regulated_building_professions", Id = "{applicationId}", PartitionKey = "{applicationId}", Connection = "CosmosConnection")]
        BuildingProfessionApplicationModel applicationModel)
    {
        var invoiceRequest = encodedRequest.GetDecodedData<NewInvoicePaymentRequestModel>()!;
        await paymentService.NewInvoicePayment(applicationModel, invoiceRequest);

        return request.CreateResponse();
    }

    [Function(nameof(GetPayment))]
    public async Task<HttpResponseData> GetPayment([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetPayment)}/{{paymentReference}}")] HttpRequestData request, string paymentReference)
    {
        if (paymentReference == null || paymentReference.Equals(string.Empty))
            return request.CreateResponse(HttpStatusCode.BadRequest);

        var dynamicsPayment = await dynamicsService.GetPaymentByReference(paymentReference);
        if (dynamicsPayment == null)
            return request.CreateResponse(HttpStatusCode.BadRequest);

        var paymentResponse = await paymentService.GetPaymentStatus(dynamicsPayment.bsr_govukpaymentid);

        return await request.CreateObjectResponseAsync(paymentResponse);
    }



    [Function(nameof(GetBuildingProfssionApplicationUsingIdActivity))]
    public async Task<DynamicsBuildingProfessionApplication> GetBuildingProfssionApplicationUsingIdActivity([ActivityTrigger] string applicationId)
    {
        return await dynamicsService.GetBuildingProfessionApplicationUsingId(applicationId);
    }

    [Function(nameof(GovukPaymentProcessed))]
    public async Task GovukPaymentProcessed([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request,
    [DurableClient] DurableTaskClient durableTaskClient,
        [CosmosDBInput("hseportal", "regulated_building_professions", Id = "{resource.metadata.applicationid}", PartitionKey = "{resource.metadata.applicationid}", Connection = "CosmosConnection")]
        BuildingProfessionApplicationModel applicationModel)
    {
        var invoiceRequest = await request.ReadAsJsonAsync<GovukPaymentEventData>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(GovukPaymentProcessedOrchestration), new GovukPaymentProcessedModel(applicationModel, invoiceRequest));
    }

    [Function(nameof(GovukPaymentProcessedOrchestration))]
    public async Task<BuildingProfessionApplicationModel> GovukPaymentProcessedOrchestration([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var model = orchestrationContext.GetInput<GovukPaymentProcessedModel>();
        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingIdActivity), model.GovukPaymentEvent.EventData.Metadata["applicationid"].ToString());

        var invoiceRequest = model.GovukPaymentEvent;
        var paymentModel = new BuildingProfessionApplicationPayment(dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid, new PaymentResponseModel
        {
            Country = invoiceRequest.EventData.CardDetails.BillingAddress.Country,
            City = invoiceRequest.EventData.CardDetails.BillingAddress.City,
            AddressLineOne = invoiceRequest.EventData.CardDetails.BillingAddress.Line1,
            AddressLineTwo = invoiceRequest.EventData.CardDetails.BillingAddress.Line2,
            CardExpiryDate = invoiceRequest.EventData.CardDetails.ExpiryDate,
            CardBrand = invoiceRequest.EventData.CardDetails.CardBrand,
            CardType = invoiceRequest.EventData.CardDetails.CardType,
            LastFourDigitsCardNumber = invoiceRequest.EventData.CardDetails.LastDigits,
            FirstDigitsCardNumber = invoiceRequest.EventData.CardDetails.FirstDigits,
            Postcode = invoiceRequest.EventData.CardDetails.BillingAddress.Postcode,
            Email = invoiceRequest.EventData.Email,
            Reference = invoiceRequest.EventData.Reference,
            Amount = invoiceRequest.EventData.Amount,
            CreatedDate = invoiceRequest.EventData.CreatedDate,
            Status = invoiceRequest.EventData.State.Status,
            PaymentId = invoiceRequest.EventData.PaymentId,
        });

        await orchestrationContext.CallActivityAsync(nameof(CreateCardPaymentActivity), paymentModel);

        var applicationModel = model.ApplicationModel;
        if (invoiceRequest?.EventType == "card_payment_succeeded")
        {
            Dictionary<string, StageCompletionState> stageStatus = applicationModel.StageStatus;
            stageStatus["Payment"] = StageCompletionState.Complete;
            applicationModel = applicationModel with { 
                ApplicationStage = ApplicationStage.ApplicationSubmitted,
                StageStatus = stageStatus
            };
            await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplicationActivity), new GovukPaymentApplicationModel(applicationModel, dynamicsBuildingProfessionApplication));
        }

        return applicationModel;
    }

    [Function(nameof(CreateCardPaymentActivity))]
    public async Task CreateCardPaymentActivity([ActivityTrigger] BuildingProfessionApplicationPayment buildingProfessionApplicationPayment)
    {
        await dynamicsService.CreatePayment(buildingProfessionApplicationPayment);
    }

    [Function(nameof(GetBuildingProfessionApplicationUsingIdActivity))]
    public async Task<DynamicsBuildingProfessionApplication> GetBuildingProfessionApplicationUsingIdActivity([ActivityTrigger] string applicationId)
    {
        return await dynamicsService.GetBuildingProfessionApplicationUsingId(applicationId);
    }

    [Function(nameof(UpdateBuildingProfessionApplicationActivity))]
    [CosmosDBOutput("hseportal", "regulated_building_professions", Connection = "CosmosConnection")]
    public async Task<BuildingProfessionApplicationModel> UpdateBuildingProfessionApplicationActivity([ActivityTrigger] GovukPaymentApplicationModel govukPaymentApplicationModel)
    {
        ApplicationStageMapper applicationStageMapper = new ApplicationStageMapper();

        BuildingProfessionApplicationStage? applicationStage = applicationStageMapper.ToBuildingApplicationStage(govukPaymentApplicationModel.BuildingProfessionApplicationModel.ApplicationStage);

        await dynamicsService.UpdateBuildingProfessionApplication(govukPaymentApplicationModel.DynamicsBuildingProfessionApplication,
            new DynamicsBuildingProfessionApplication { bsr_buildingprofessionalapplicationstage = applicationStage});

        return govukPaymentApplicationModel.BuildingProfessionApplicationModel;
    }

    public record GovukPaymentProcessedModel(BuildingProfessionApplicationModel ApplicationModel, GovukPaymentEventData GovukPaymentEvent);

}