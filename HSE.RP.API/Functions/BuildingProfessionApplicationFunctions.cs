using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using HSE.RP.API.Extensions;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

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


    [Function(nameof(ValidateApplicationNumberEmail))]
    public Task<HttpResponseData> ValidateApplicationNumberEmail([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ValidateApplicationNumberEmail/{EmailAddress}/{ApplicationNumber}")] HttpRequestData request,
    [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE c.id = UPPER({applicationNumber})", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> applications,
    [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE StringEquals(c.PersonalDetails.ApplicantEmail.Email, {EmailAddress}, true)", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> emails)

    {


        if (applications.Any() && emails.Any(app => app.Id == applications[0].Id))
        {
            return request.CreateObjectResponseAsync(new { IsValid = true, IsValidApplicationNumber = true, PhoneNumber = applications[0].PersonalDetails.ApplicantPhone.PhoneNumber, EmailAddress = "" });
        }
        else if (emails.Any())
        {
            return request.CreateObjectResponseAsync(new { IsValid = true, IsValidApplicationNumber = false, PhoneNumber = "", EmailAddress = "" }); ;
        }
        else if (applications.Any())
        {
            return request.CreateObjectResponseAsync(new { IsValid = false, IsValidApplicationNumber = true, PhoneNumber = applications[0].PersonalDetails.ApplicantPhone.PhoneNumber, EmailAddress = "" }); ;
        }
        else
        {
            return request.CreateObjectResponseAsync(new { IsValid = false, isValiIsValidApplicationNumberdApplicationNumber = false, PhoneNumber = "", EmailAddress = "" }); ;
        }
    }

    [Function(nameof(ValidateApplicationNumberPhone))]
    public Task<HttpResponseData> ValidateApplicationNumberPhone([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ValidateApplicationNumberPhone/{PhoneNumber}/{ApplicationNumber}")] HttpRequestData request,
    [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE c.id = UPPER({applicationNumber})", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> applications,
    [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE StringEquals(c.PersonalDetails.ApplicantPhone.PhoneNumber, {PhoneNumber}, true)", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> phoneNumbers)

    {


        if (applications.Any() && phoneNumbers.Any(app => app.Id == applications[0].Id))
        {
            return request.CreateObjectResponseAsync(new { IsValid = true, IsValidApplicationNumber = true, EmailAddress = applications[0].PersonalDetails.ApplicantEmail.Email, PhoneNumber = "" });
        }
        else if (phoneNumbers.Any())
        {
            return request.CreateObjectResponseAsync(new { IsValid = true, IsValidApplicationNumber = false, EmailAddress = "", PhoneNumber = "" }); ;
        }
        else if (applications.Any())
        {
            return request.CreateObjectResponseAsync(new { IsValid = false, IsValidApplicationNumber = true, EmailAddress = applications[0].PersonalDetails.ApplicantEmail.Email, PhoneNumber = "" }); ;
        }
        else
        {
            return request.CreateObjectResponseAsync(new { IsValid = false, IsValidApplicationNumber = false, EmailAddress = "", PhoneNumber = "" }); ;
        }
    }


    [Function(nameof(GetApplication))]
    public async Task<HttpResponseData> GetApplication([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetApplication/{applicationNumber}/{emailAddress}/{otpToken}")] HttpRequestData request,
        [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE c.id = UPPER({applicationNumber}) and c.PersonalDetails.ApplicantEmail.Email = {emailAddress}", PartitionKey = "{applicationNumber}", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> buildingProfessionApplications, string otpToken)
    {
        if (buildingProfessionApplications.Any())
        {
            var application = buildingProfessionApplications[0];
            if (await otpService.ValidateToken(otpToken, application.PersonalDetails.ApplicantEmail.Email) || featureOptions.DisableOtpValidation)
            {
                return await request.CreateObjectResponseAsync(application);
            }
        }

        return request.CreateResponse(HttpStatusCode.BadRequest);
    }

    [Function(nameof(GetApplicationPhone))]
    public async Task<HttpResponseData> GetApplicationPhone([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetApplicationPhone/{ApplicationNumber}/{PhoneNumber}/{OTPToken}")] HttpRequestData request,
    [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE c.id = UPPER({applicationNumber}) and c.PersonalDetails.ApplicantPhone.PhoneNumber = {PhoneNumber}", PartitionKey = "{applicationNumber}", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> buildingProfessionApplications, string otpToken)
    {
        if (buildingProfessionApplications.Any())
        {
            var application = buildingProfessionApplications[0];
            if (await otpService.ValidateToken(otpToken, application.PersonalDetails.ApplicantPhone.PhoneNumber)|| featureOptions.DisableOtpValidation)
            {
                return await request.CreateObjectResponseAsync(application);
            }
        }

        return request.CreateResponse(HttpStatusCode.BadRequest);
    }

    [Function(nameof(GetApplicationEmail))]
    public async Task<HttpResponseData> GetApplicationEmail([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetApplicationEmail/{ApplicationNumber}/{EmailAddress}/{OTPToken}")] HttpRequestData request,
    [CosmosDBInput("hseportal", "regulated_building_professions", SqlQuery = "SELECT * FROM c WHERE c.id = UPPER(applicationNumber}) and c.PersonalDetails.ApplicantEmail.Email = {EmailAddress}", PartitionKey = "{applicationNumber}", Connection = "CosmosConnection")]
        List<BuildingProfessionApplicationModel> buildingProfessionApplications, string otpToken)
    {
        if (buildingProfessionApplications.Any())
        {
            var application = buildingProfessionApplications[0];
            if (await otpService.ValidateToken(otpToken, application.PersonalDetails.ApplicantEmail.Email) || featureOptions.DisableOtpValidation)
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

    [Function(nameof(GetApplicationPaymentStatus))]
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
    }
}

public class CustomHttpResponseData
{
    [CosmosDBOutput("hseportal", "regulated_building_professions", Connection = "CosmosConnection")]
    public object Application { get; set; }

    public HttpResponseData HttpResponse { get; set; }
}