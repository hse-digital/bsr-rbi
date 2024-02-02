using Flurl;
using Flurl.Http;
using HSE.RP.API.Extensions;
using HSE.RP.API.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;
using System.Net;

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
                .SetQueryParam("includeAllUKAddresses", request.Query.Get("includeAllUKAddresses"))
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
            .SetQueryParam("includeAllUKAddresses", request.Query.Get("includeAllUKAddresses"))
            .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
            .AllowHttpStatus(HttpStatusCode.BadRequest)
            .GetAsync();

        var stream = await resp.GetStreamAsync();

        return await request.CreateObjectResponseFromStreamAsync(stream);

    }

    [Function(nameof(SearchAllAddressByPostcode))]
    public async Task<HttpResponseData> SearchAllAddressByPostcode([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(SearchAllAddressByPostcode)}/{{postcode}}")] HttpRequestData request, string postcode)
    {
        var resp = await integrationOptions.CommonAPIEndpoint
            .AppendPathSegment("api")
            .AppendPathSegment(nameof(SearchAllAddressByPostcode))
            .AppendPathSegment(postcode)
            .SetQueryParam("includeAllUKAddresses", request.Query.Get("includeAllUKAddresses"))
            .WithHeader("x-functions-key", integrationOptions.CommonAPIKey)
            .AllowHttpStatus(HttpStatusCode.BadRequest)
            .GetAsync();

        var stream = await resp.GetStreamAsync();

        return await request.CreateObjectResponseFromStreamAsync(stream);

    }
}
