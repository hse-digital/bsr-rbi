using System.Net;
using System.Text.RegularExpressions;
using AutoMapper;
using Flurl;
using Flurl.Http;
using HSE.RP.API.Extensions;
using HSE.RP.API.Models;
using HSE.RP.API.Models.DynamicsSynchronisation;
using HSE.RP.API.Models.Payment.Request;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.API.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Functions;

public class PaymentFunctions
{
    private readonly DynamicsService dynamicsService;
    private readonly IMapper mapper;
    private readonly IntegrationsOptions integrationOptions;
    private readonly SwaOptions swaOptions;

    public PaymentFunctions(IOptions<IntegrationsOptions> integrationOptions, IOptions<SwaOptions> swaOptions, DynamicsService dynamicsService, IMapper mapper)
    {
        this.dynamicsService = dynamicsService;
        this.mapper = mapper;
        this.integrationOptions = integrationOptions.Value;
        this.swaOptions = swaOptions.Value;
    }

    [Function(nameof(InitialisePayment))]
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
    }

    private static PaymentRequestModel BuildPaymentRequestModel(BuildingProfessionApplicationModel applicationModel)
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
                    Line1 = address?.Address ?? "Buckingham Palace",
                    Line2 = address?.AddressLineTwo ?? "London",
                    Postcode = address?.Postcode ?? "SW1A 1AA",
                    City = address?.Town ?? "London"
                }
            }
        };
        return paymentModel;
    }


    [Function(nameof(GetPayment))]
    public async Task<HttpResponseData> GetPayment([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetPayment)}/{{paymentReference}}")] HttpRequestData request, string paymentReference)
    {
        if (paymentReference == null || paymentReference.Equals(string.Empty))
            return request.CreateResponse(HttpStatusCode.BadRequest);

        var dynamicsPayment = await dynamicsService.GetPaymentByReference(paymentReference);
        if (dynamicsPayment == null)
            return request.CreateResponse(HttpStatusCode.BadRequest);

        var response = await integrationOptions.PaymentEndpoint
            .AppendPathSegments("v1", "payments", dynamicsPayment.bsr_govukpaymentid)
            .WithOAuthBearerToken(integrationOptions.PaymentApiKey)
            .AllowHttpStatus(HttpStatusCode.BadRequest)
            .GetJsonAsync<PaymentApiResponseModel>();

        var paymentFunctionResponse = mapper.Map<PaymentResponseModel>(response);
        return await request.CreateObjectResponseAsync(paymentFunctionResponse);
    }



}