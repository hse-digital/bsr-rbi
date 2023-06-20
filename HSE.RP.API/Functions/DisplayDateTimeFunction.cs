using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using HSE.RP.API.Extensions;

namespace HSE.RP.API.Functions;

public class DisplayDateTimeFunction
{
    [Function(nameof(DisplayDateTime))]
    public async Task<HttpResponseData> DisplayDateTime([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(DisplayDateTime)}")] HttpRequestData request)
    {
        return await GetDateTime(request);
    }

    private async Task<HttpResponseData> GetDateTime(HttpRequestData request)
    {
        var dateTime = DateTime.UtcNow.ToString();
        return await request.CreateObjectResponseAsync(dateTime);
    }

}
