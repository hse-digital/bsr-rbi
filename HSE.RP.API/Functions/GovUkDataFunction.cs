using HSE.RP.API.BlobStore;
using HSE.RP.API.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Functions
{
    public class GovUkDataFunction
    {
        private readonly IGovUKDataSASUri _govUKDataUri;
        private readonly GovUKDataOptions _govUKDataOptions;
        private readonly TimeSpan _ttlMinutes;

        public GovUkDataFunction(IGovUKDataSASUri govUKDataUri, IOptions<GovUKDataOptions> govUkDataOptions)
        {
            this._govUKDataUri = govUKDataUri;
            this._govUKDataOptions = govUkDataOptions.Value;
            _ttlMinutes = TimeSpan.FromMinutes(govUkDataOptions.Value.TTLMinutes);
        }

        [Function(nameof(GetGovUKDataUri))]
        public async Task<HttpResponseData> GetGovUKDataUri([HttpTrigger(AuthorizationLevel.Anonymous, "get",
        Route = $"{nameof(GetGovUKDataUri)}")] HttpRequestData request)
        {
            var blobString = _govUKDataUri.GetReadableGovUKDataSASUri($"{_govUKDataOptions.ContainerName}/{_govUKDataOptions.BlobName}", _ttlMinutes);

            return await request.CreateObjectResponseAsync(blobString);
        }
    }
}
