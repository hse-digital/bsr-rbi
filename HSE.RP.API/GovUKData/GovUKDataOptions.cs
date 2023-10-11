namespace HSE.RP.API.GovUKData
{
    public class GovUKDataOptions
    {
        public const string GovUKDataStore = "GovUKDataStore";

        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public int TTLMinutes { get; set; }
    }
}
