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
using Azure.Core;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace HSE.RPR.API.Functions
{
    public interface IRegisterFunctions
    {
        Task<HttpResponseData> SearchRBIInspectorNames([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req);
        Task<HttpResponseData> SearchRBICompanyNames([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req);
        Task<HttpResponseData> SearchRBIRegister([HttpTrigger(AuthorizationLevel.Function, "get",  Route = null)] HttpRequestData req);
        Task<HttpResponseData> GetRBIDetails([HttpTrigger(AuthorizationLevel.Anonymous, new[] { "get" }, Route = "GetRBIDetails/{Id}")] HttpRequestData request, string Id);
    }

    public class RegisterFunctions : IRegisterFunctions
    {
        private readonly ICosmosDbService cosmosDbService;
        private readonly IRegisterSearchService registerSearchService;



        public RegisterFunctions(ICosmosDbService cosmosDbService, IRegisterSearchService registerSearchService)
        {
            this.cosmosDbService = cosmosDbService;
            this.registerSearchService = registerSearchService;
        }


        [Function(nameof(SearchRBIInspectorNames))]
        public async Task<HttpResponseData> SearchRBIInspectorNames([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
        {
            var parameters = request.GetQueryParameters();

            //extract the country and name from request body


            var country = parameters["country"];
            var name = parameters["name"];

            if (string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(name))
                return request.CreateResponse(HttpStatusCode.BadRequest);

            var inspectorNames = await registerSearchService.GetRBIInspectorsByNameAndCountry(name, country);

            // Check if nameResponse contains applications
            if (inspectorNames.Inspectors.Any())
            {
                return await request.CreateObjectResponseAsync(inspectorNames);
            }
            else return request.CreateResponse(HttpStatusCode.NotFound);
        }

        [Function(nameof(SearchRBICompanyNames))]
        public async Task<HttpResponseData> SearchRBICompanyNames([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
        {
            var parameters = request.GetQueryParameters();

            //extract the country and name from request body


            var country = parameters["country"];
            var company = parameters["company"];

            if (string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(company))
                return request.CreateResponse(HttpStatusCode.BadRequest);

            var companyNames = await registerSearchService.GetRBICompaniesByNameAndCountry(company, country);

            // Check if nameResponse contains applications
            if (companyNames.Companies.Any())
            {
                return await request.CreateObjectResponseAsync(companyNames);
            }
            else return request.CreateResponse(HttpStatusCode.NotFound);

        }


        [Function(nameof(GetRegisterLastUpdated))]
        public async Task<HttpResponseData> GetRegisterLastUpdated([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
        {
            var parameters = request.GetQueryParameters();

            //extract the country and name from request body


            var country = parameters["country"];
            var service = parameters["service"];

            if (string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(service))
                return request.CreateResponse(HttpStatusCode.BadRequest);

            var lastUpdateDate = await registerSearchService.GetRegisterLastUpdated(service, country);

            // Check if nameResponse contains applications

            return await request.CreateObjectResponseAsync(lastUpdateDate);

        }
        [Function(nameof(SearchRBIRegister))]

        public async Task<HttpResponseData> SearchRBIRegister([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
        {
            var parameters = request.GetQueryParameters();


            var country = parameters["country"] ?? "";
            var name = parameters["name"] ?? "";
            var company = parameters["company"] ?? "";


            if (string.IsNullOrWhiteSpace(country))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(company))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var searchResults = await registerSearchService.SearchRBIRegister(name, company, country);


            if (searchResults.RBIApplications.Any())
            {
                return await request.CreateObjectResponseAsync(searchResults);
            }
            else return request.CreateResponse(HttpStatusCode.NotFound);

        }


        [Function(nameof(GetRBIDetails))]
        public async Task<HttpResponseData> GetRBIDetails([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetRBIDetails)}/{{Id}}")] HttpRequestData request, string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var rbiDetails = await registerSearchService.GetRBIDetails(Id);

            if (rbiDetails != null)
            {
                return await request.CreateObjectResponseAsync(rbiDetails);
            }
            else return request.CreateResponse(HttpStatusCode.NotFound);

        }

    }



}
