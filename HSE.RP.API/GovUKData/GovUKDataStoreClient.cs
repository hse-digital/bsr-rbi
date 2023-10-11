// Doesn't look like were using this?



//using Azure.Storage.Blobs;
//using Microsoft.Extensions.Options;

//namespace HSE.RP.API.BlobStore;

//public interface IGovUKDataStoreClient
//{
//    BlobServiceClient GetGovUKDataClient();
//}
//public class GovUKDataStoreClient : IGovUKDataStoreClient
//{
//    private readonly GovUKDataOptions _govUKDataOptions;

//    public GovUKDataStoreClient(IOptions<GovUKDataOptions> govUKDataOptions)
//    {
//        this._govUKDataOptions = govUKDataOptions.Value;
//    }

//    public BlobServiceClient GetGovUKDataClient()
//    {
//        return new BlobServiceClient(_govUKDataOptions.ConnectionString);
//    }
//}
