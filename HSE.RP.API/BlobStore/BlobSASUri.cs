using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.BlobStore
{
    public interface IBlobSASUri
    {
        string GetWritableSASUri(string blobName, TimeSpan ttl);
        string GetWritableSASUri(string blobName) => GetWritableSASUri(blobName, TimeSpan.FromHours(1));

        string GetReadableSASUri(string blobName, TimeSpan ttl);
        string GetReadableSASUri(string blobName) => GetReadableSASUri(blobName, TimeSpan.FromHours(1));
    }

    public class BlobSASUri : IBlobSASUri
    {
        
        private readonly BlobStoreOptions blobStoreOptions;

        public BlobSASUri(IOptions<BlobStoreOptions> blobStoreOptions)
        {
            this.blobStoreOptions = blobStoreOptions.Value;
        }

        public string GetReadableSASUri(string blobName, TimeSpan ttl)
        {
            return GetSASUri(blobName, BlobSasPermissions.Read, ttl);
        }

        public string GetWritableSASUri(string blobName, TimeSpan ttl)
        {
            return GetSASUri(blobName, BlobSasPermissions.Write | BlobSasPermissions.Delete, ttl);
        }

        private string GetSASUri(string blobName, BlobSasPermissions permissions, TimeSpan ttl)
        {
            var blobServiceClient = new BlobServiceClient(blobStoreOptions.ConnectionString);

            //  Gets a reference to the container.
            var containerClient = blobServiceClient.GetBlobContainerClient(blobStoreOptions.ContainerName);

            //  Gets a reference to the blob in the container
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            Uri fullUri = blobClient.GenerateSasUri(permissions, DateTime.UtcNow.Add(ttl));

            return fullUri.ToString();
        }
    }

}

