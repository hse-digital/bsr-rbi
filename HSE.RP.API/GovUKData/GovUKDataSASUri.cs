using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.GovUKData
{
    public interface IGovUKDataSASUri
    {
        string GetWritableGovUKDataSASUri(string blobName, TimeSpan ttl);
        string GetWritableGovUKDataSASUri(string blobName) => GetWritableGovUKDataSASUri(blobName, TimeSpan.FromMinutes(1));

        string GetReadableGovUKDataSASUri(string blobName, TimeSpan ttl);
        string GetReadableGovUKDataSASUri(string blobName) => GetReadableGovUKDataSASUri(blobName, TimeSpan.FromMinutes(1));
    }

    public class GovUKDataSASUri : IGovUKDataSASUri
    {
        private readonly GovUKDataOptions govUKDataOptions;

        public GovUKDataSASUri(IOptions<GovUKDataOptions> govUKDataOptions)
        {
            this.govUKDataOptions = govUKDataOptions.Value;
        }

        public string GetReadableGovUKDataSASUri(string blobName, TimeSpan ttl)
        {
            return GetGovUKDataUri(blobName, BlobSasPermissions.Read, ttl);
        }

        public string GetWritableGovUKDataSASUri(string blobName, TimeSpan ttl)
        {
            return GetGovUKDataUri(blobName, BlobSasPermissions.Write | BlobSasPermissions.Delete, ttl);
        }

        private string GetGovUKDataUri(string blobName, BlobSasPermissions permissions, TimeSpan ttl)
        {
            var blobServiceClient = new BlobServiceClient(govUKDataOptions.ConnectionString);

            //  Gets a reference to the container.
            var containerClient = blobServiceClient.GetBlobContainerClient(govUKDataOptions.ContainerName);

            //  Gets a reference to the blob in the container
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            Uri fullUri = blobClient.GenerateSasUri(permissions, DateTime.UtcNow.Add(ttl));

            return fullUri.ToString();
        }
    }
}
