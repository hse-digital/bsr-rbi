namespace HSE.RP.API.BlobStore
{
    public class BlobStoreOptions
    {
        public const string BlobStore = "BlobStore";
        
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
    }
}
