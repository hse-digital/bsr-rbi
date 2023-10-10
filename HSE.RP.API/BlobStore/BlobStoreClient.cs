using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.BlobStore;

public interface IBlobClient
{
    BlobServiceClient GetBlobServiceClient();
}
public class BlobStoreClient : IBlobClient
{
    private readonly BlobStoreOptions blobStoreOptions;

    public BlobStoreClient(IOptions<BlobStoreOptions> blobStoreOptions)
    {
        this.blobStoreOptions = blobStoreOptions.Value;
    }
    public BlobServiceClient GetBlobServiceClient()
    {
        return new BlobServiceClient(blobStoreOptions.ConnectionString);

    }
}
