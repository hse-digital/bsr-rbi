using HSE.RP.API.Extensions;
using HSE.RP.API.GovUKData;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Functions
{
    public class GovUKDataFunction
    {
        private readonly IGovUKDataSASUri _govUKDataUri;
        private readonly GovUKDataOptions _govUKDataOptions;
        private readonly TimeSpan _ttlMinutes;

        public GovUKDataFunction(IGovUKDataSASUri govUKDataUri, IOptions<GovUKDataOptions> govUkDataOptions)
        {
            this._govUKDataUri = govUKDataUri;
            this._govUKDataOptions = govUkDataOptions.Value;
            _ttlMinutes = TimeSpan.FromMinutes(govUkDataOptions.Value.TTLMinutes);
        }

        [Function(nameof(GetGovUKDataSASUri))]
        public async Task<HttpResponseData> GetGovUKDataSASUri([HttpTrigger(AuthorizationLevel.Anonymous, "get",
        Route = $"{nameof(GetGovUKDataSASUri)}")] HttpRequestData request)
        {
            var blobString = _govUKDataUri.GetReadableGovUKDataSASUri(_govUKDataOptions.BlobName, _ttlMinutes);

            return await request.CreateObjectResponseAsync(blobString);
        }
    }
}
