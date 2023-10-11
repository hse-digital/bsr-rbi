using HSE.RP.API.BlobStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HSE.RP.API.GovUKData
{
    public static class GovUKDataDependencyInjectionExtensions
    {
        public static IServiceCollection AddGovUKData(this IServiceCollection services, HostBuilderContext builderContext)
        {
            services.Configure<GovUKDataOptions>(builderContext.Configuration.GetSection(GovUKDataOptions.GovUKDataStore));
            services.AddSingleton<IGovUKDataSASUri, GovUKDataSASUri>();         
            
            return services;
        }
    }
}
