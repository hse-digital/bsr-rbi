using System.Diagnostics;
using System.Net;
using HSE.RP.API.Extensions;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Functions;

public class BuildingProfessionApplicationFunctions
{
    private readonly DynamicsService dynamicsService;
    private readonly OTPService otpService;
    private readonly FeatureOptions featureOptions;

    public BuildingProfessionApplicationFunctions(DynamicsService dynamicsService, OTPService otpService, IOptions<FeatureOptions> featureOptions)
    {
        this.dynamicsService = dynamicsService;
        this.otpService = otpService;
        this.featureOptions = featureOptions.Value;
    }

    [Function(nameof(NewBuildingProfessionalApplication))]
    public async Task<CustomHttpResponseData> NewBuildingProfessionalApplication([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
    {
        var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var validation = buildingProfessionApplicationModel.Validate();
        if (!featureOptions.DisableOtpValidation && !validation.IsValid)
        {
            return await request.BuildValidationErrorResponseDataAsync(validation);
        }

        buildingProfessionApplicationModel = await dynamicsService.RegisterNewBuildingProfessionApplicationAsync(buildingProfessionApplicationModel);
        var response = await request.CreateObjectResponseAsync(buildingProfessionApplicationModel);
        return new CustomHttpResponseData
        {
            Application = buildingProfessionApplicationModel,
            HttpResponse = response
        };
    }

    [Function(nameof(ValidateApplicationNumber))]
    public HttpResponseData ValidateApplicationNumber([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ValidateApplicationNumber/{emailAddress}/{applicationNumber}")] HttpRequestData request,
        [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE c.id = {applicationNumber} and StringEquals(c.PersonalDetails.ApplicantEmail, {emailAddress}, true)", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> buildingProfessionApplications)
    {
        return request.CreateResponse(buildingProfessionApplications.Any() ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
    }

/*    [Function(nameof(GetSubmissionDate))]
    public async Task<HttpResponseData> GetSubmissionDate([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetSubmissionDate/{applicationNumber}")] HttpRequestData request, string applicationNumber)
    {
        string submissionDate = await dynamicsService.GetSubmissionDate(applicationNumber);
        return await request.CreateObjectResponseAsync(submissionDate);
    }*/

    [Function(nameof(GetApplication))]
    public async Task<HttpResponseData> GetApplication([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetApplication/{applicationNumber}/{emailAddress}/{otpToken}")] HttpRequestData request,
        [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE c.id = {applicationNumber} and c.PersonalDetails.ApplicantEmail = {emailAddress}", PartitionKey = "{applicationNumber}", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> buildingProfessionApplications, string otpToken)
    {
        if (buildingProfessionApplications.Any())
        {
            var application = buildingProfessionApplications[0];
            if (otpService.ValidateToken(otpToken, application.PersonalDetails.ApplicantEmail) || featureOptions.DisableOtpValidation)
            {
                return await request.CreateObjectResponseAsync(application);
            }
        }

        return request.CreateResponse(HttpStatusCode.BadRequest);
    }

    [Function(nameof(UpdateApplication))]
    public async Task<CustomHttpResponseData> UpdateApplication([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "UpdateApplication/{applicationNumber}")] HttpRequestData request)
    {
        var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var validation = buildingProfessionApplicationModel.Validate();
        if (!validation.IsValid)
        {
            return await request.BuildValidationErrorResponseDataAsync(validation);
        }

        return new CustomHttpResponseData
        {
            Application = buildingProfessionApplicationModel,
            HttpResponse = request.CreateResponse(HttpStatusCode.OK)
        };
    }

/*    [Function(nameof(GetApplicationPaymentStatus))]
    public async Task<HttpResponseData> GetApplicationPaymentStatus([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetApplicationPaymentStatus)}/{{applicationNumber}}")] HttpRequestData request,
        string applicationNumber)
    {
        var dynamicsPayments = await dynamicsService.GetPayments(applicationNumber);
        var payments = dynamicsPayments.Select(payment => new
        {
            payment.bsr_paymentid,
            payment.bsr_govukpaystatus,
            payment.bsr_paymentreconciliationstatus,
            payment.bsr_amountpaid,
            payment.bsr_transactionid,
            payment.bsr_timeanddateoftransaction
        });
        
        return await request.CreateObjectResponseAsync(payments);
    }*/
}

public class CustomHttpResponseData
{
    [CosmosDBOutput("hseportal", "regulated_building_professions", Connection = "CosmosConnection")]
    public object Application { get; set; }

    public HttpResponseData HttpResponse { get; set; }
}