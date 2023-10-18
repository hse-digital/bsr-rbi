using AutoMapper;
using HSE.RP.API.Extensions;
using HSE.RP.API.Mappers;
using HSE.RP.API.Models;
using HSE.RP.API.Models.Payment;
using HSE.RP.API.Models.Payment.Request;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.Extensions.Options;
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

    /*    [Function(nameof(InitialisePayment))]
        public async Task<HttpResponseData> InitialisePayment([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = $"{nameof(InitialisePayment)}/{{applicationId}}")] HttpRequestData request,
            [CosmosDBInput("hseportal", "regulated_building_professions", Id = "{applicationId}", PartitionKey = "{applicationId}", Connection = "CosmosConnection")]
            BuildingProfessionApplicationModel applicationModel)
        {
            var paymentModel = BuildPaymentRequestModel(applicationModel);


            var validation = paymentModel.Validate();
            if (!validation.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var paymentRequestModel = mapper.Map<PaymentApiRequestModel>(paymentModel);
            paymentRequestModel.description = $"Payment for application {applicationModel.Id}";
            paymentRequestModel.amount = integrationOptions.PaymentAmount;
            paymentRequestModel.return_url = $"{swaOptions.Url}/application/{applicationModel.Id}/application-submission/payment/confirm?reference={paymentModel.Reference}";

            var response = await integrationOptions.PaymentEndpoint
                .AppendPathSegments("v1", "payments")
                .WithOAuthBearerToken(integrationOptions.PaymentApiKey)
                .PostJsonAsync(paymentRequestModel);

            if (response.StatusCode == (int)HttpStatusCode.BadRequest)
                return request.CreateResponse(HttpStatusCode.BadRequest);

            var paymentApiResponse = await response.GetJsonAsync<PaymentApiResponseModel>();
            var paymentResponse = mapper.Map<PaymentResponseModel>(paymentApiResponse);
            await dynamicsService.NewPayment(applicationModel.Id, paymentResponse);

            return await request.CreateObjectResponseAsync(paymentResponse);
        }*/

    /*private static PaymentRequestModel BuildPaymentRequestModel(BuildingProfessionApplicationModel applicationModel)
    {
        var address = applicationModel.PersonalDetails.ApplicantAddress;
        var paymentModel = new PaymentRequestModel
        {
            Reference = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..22], @"\W", "0"),
            Email = applicationModel.PersonalDetails.ApplicantEmail.Email,
            CardHolderDetails = new CardHolderDetails
            {
                Name = $"{applicationModel.PersonalDetails.ApplicantName.FirstName} {applicationModel.PersonalDetails.ApplicantName.LastName}",
                Address = new CardHolderAddress
                {
                    Line1 = address?.Address ?? "",
                    Line2 = address?.AddressLineTwo ?? "",
                    Postcode = address?.Postcode ?? "",
                    City = address?.Town ?? ""
                }
            }
        };
        return paymentModel;
    }*/


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


    [Function(nameof(GovukPaymentProcessedOrchestration))]
    public async Task GovukPaymentProcessedOrchestration([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var model = orchestrationContext.GetInput<GovukPaymentProcessedModel>();
        var dynamicsBuildingApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfssionApplicationUsingIdActivity), model.GovukPaymentEvent.EventData.Metadata["applicationid"].ToString());


    }

    [Function(nameof(GetBuildingProfssionApplicationUsingIdActivity))]
    public async Task<DynamicsBuildingProfessionApplication> GetBuildingProfssionApplicationUsingIdActivity([ActivityTrigger] string applicationId)
    {
        return await dynamicsService.GetBuildingProfessionApplicationUsingId(applicationId);
    }

    public record GovukPaymentProcessedModel(BuildingProfessionApplication ApplicationModel, GovukPaymentEventData GovukPaymentEvent);

}