using AutoMapper;
using HSE.RP.API.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using System.Net;
using HSE.RP.API.Extensions;

namespace HSE.RP.API.Functions;

public class AddressFunctions
{
    private readonly IntegrationsOptions integrationOptions;
    public AddressFunctions(IOptions<IntegrationsOptions> integrationOptions)
    {
        this.integrationOptions = integrationOptions.Value;
    }

    [Function(nameof(SearchPostalAddressByPostcode))]
    public async Task<HttpResponseData> SearchPostalAddressByPostcode([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(SearchPostalAddressByPostcode)}/{{postcode}}")] HttpRequestData request, string postcode)
    {
        try
        {
            var resp = await integrationOptions.CommonAPIEndpoint
                .AppendPathSegment("api")
                .AppendPathSegment(nameof(SearchPostalAddressByPostcode))
                .AppendPathSegment(postcode)
                .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
                .AllowHttpStatus(HttpStatusCode.BadRequest)
                .GetAsync();

            var stream = await resp.GetStreamAsync();

            return await request.CreateObjectResponseFromStreamAsync(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }

    [Function(nameof(SearchAddress))]
    public async Task<HttpResponseData> SearchAddress([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(SearchAddress)}/{{query}}")] HttpRequestData request, string query)
    {
        var resp = await integrationOptions.CommonAPIEndpoint
                .AppendPathSegment("api")
            .AppendPathSegment(nameof(SearchAddress))
            .SetQueryParam("query", query)
            .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
            .AllowHttpStatus(HttpStatusCode.BadRequest)
            .GetAsync();

        var stream = await resp.GetStreamAsync();

        return await request.CreateObjectResponseFromStreamAsync(stream);

    }
}
