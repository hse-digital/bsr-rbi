using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Flurl.Http;
using Flurl.Http.Configuration;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using HSE.RP.Domain.DynamicsDefinitions;
using HSEPortal.API.Models.Payment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(ConfigureServices)
    .Build();

FlurlHttp.Configure(settings => { settings.JsonSerializer = new SystemTextJsonSerializer(); });

host.Run();


static void ConfigureServices(HostBuilderContext builderContext, IServiceCollection serviceCollection)
{
    serviceCollection.Configure<DynamicsOptions>(builderContext.Configuration.GetSection(DynamicsOptions.Dynamics));
    serviceCollection.Configure<IntegrationsOptions>(builderContext.Configuration.GetSection(IntegrationsOptions.Integrations));
    serviceCollection.Configure<FeatureOptions>(builderContext.Configuration.GetSection(FeatureOptions.Feature));
    serviceCollection.Configure<SwaOptions>(builderContext.Configuration.GetSection(SwaOptions.Swa));
    serviceCollection.AddTransient<DynamicsModelDefinitionFactory>();



    serviceCollection.AddTransient<DynamicsService>();
    serviceCollection.AddTransient<DynamicsApi>();
    serviceCollection.AddTransient<OTPService>();
    serviceCollection.AddTransient<NotificationService>();

    serviceCollection.AddSingleton(_ => new MapperConfiguration(config =>
    {
        //config.AddProfile<OrdnanceSurveyPostcodeResponseProfile>();
        //config.AddProfile<CompaniesHouseSearchResponseProfile>();
        //config.AddProfile<LocalAuthoritiesSearchResponseProfile>();
        config.AddProfile<PaymentProfile>();
    }).CreateMapper());
}



public class SystemTextJsonSerializer : ISerializer
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    public string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj, serializerOptions);
    }

    public T Deserialize<T>(string s)
    {
        return JsonSerializer.Deserialize<T>(s, serializerOptions);
    }

    public T Deserialize<T>(Stream stream)
    {
        return JsonSerializer.Deserialize<T>(stream, serializerOptions);
    }
}