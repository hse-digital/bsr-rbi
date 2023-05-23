using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace HSE.PAC.API.Extensions;

public static class HttpRequestDataExtensions
{
    public static async Task<HttpResponseData> CreateObjectResponseAsync<T>(this HttpRequestData httpRequestData, T @object)
    {
        var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, @object);

        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);

        var response = httpRequestData.CreateResponse(HttpStatusCode.OK);
        response.Body = stream;

        return response;
    }
}
