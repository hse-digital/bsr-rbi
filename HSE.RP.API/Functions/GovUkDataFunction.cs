using HSE.RP.API.BlobStore;
using HSE.RP.API.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Functions
{
    public class GovUkDataFunction
    {
        private readonly IBlobSASUri blobSASUri;
        private readonly BlobStoreOptions blobStoreOptions;

        public GovUkDataFunction(IBlobSASUri blobSASUri, IOptions<BlobStoreOptions> blobStoreOptions)
        {
            this.blobSASUri = blobSASUri;
            this.blobStoreOptions = blobStoreOptions.Value;
        }

        [Function(nameof(GetSASUri))]
        public async Task<HttpResponseData> GetSASUri([HttpTrigger(AuthorizationLevel.Anonymous, "get",
        Route = $"{nameof(GetSASUri)}")] HttpRequestData request)
        {
            var blobString = blobSASUri.GetReadableSASUri($"{blobStoreOptions.ContainerName}/{blobStoreOptions.BlobName}");

            return await request.CreateObjectResponseAsync(blobString);
        }
    }
}
