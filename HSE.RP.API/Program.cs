using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Flurl.Http;
using Flurl.Http.Configuration;
using HSE.RP.API.Enums;
using HSE.RP.API.Extensions;
using HSE.RP.API.GovUKData;
using HSE.RP.API.Models;
using HSE.RP.API.Models.CompaniesHouse;
using HSE.RP.API.Models.LocalAuthority;
using HSE.RP.API.Models.OrdnanceSurvey;
using HSE.RP.API.Services;
using HSE.RP.API.Services.CompanySearch;
using HSE.RP.Domain.DynamicsDefinitions;
using HSEPortal.API.Models.Payment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerOptions =>
    {
        workerOptions.InputConverters.Register<EncodedRequestConverter>();
    })
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
    serviceCollection.AddTransient<CompanySearchService>();
    serviceCollection.AddTransient<CompanySearchFactory>();


    serviceCollection.AddSingleton(_ => new MapperConfiguration(config =>
    {
        config.AddProfile<OrdnanceSurveyPostcodeResponseProfile>();
        config.AddProfile<PaymentProfile>();
        config.AddProfile<CompaniesHouseSearchResponseProfile>();
        config.AddProfile<LocalAuthoritiesSearchResponseProfile>();

    }).CreateMapper());

    serviceCollection.AddGovUKData(builderContext);
}



public class SystemTextJsonSerializer : ISerializer
{
    public class JsonStringEnumConverter : JsonConverter<ComponentCompletionState>
    {
        public override ComponentCompletionState Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            string enumString = reader.GetString();

            // Try parsing the enum string
            if (System.Enum.TryParse(enumString, out ComponentCompletionState enumValue))
            {
                return enumValue;
            }
            else
            {
                return default;
            }
        }

        public override void Write(Utf8JsonWriter writer, ComponentCompletionState value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    private readonly JsonSerializerOptions serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    public SystemTextJsonSerializer()
    {
        serializerOptions.Converters.Add(new JsonStringEnumConverter());

    }
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