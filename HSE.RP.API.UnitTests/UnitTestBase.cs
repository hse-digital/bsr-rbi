using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Flurl.Http;
using Flurl.Http.Testing;
using HSE.RP.API.Models;
using HSE.RP.API.Services;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Options;
using Moq;
using Flurl;
using HSE.RP.API.UnitTests.Helpers;
using System.Text.Json;
using HSE.RP.API.Models.OrdnanceSurvey;
using HSE.RP.API.Models.CompaniesHouse;
using HSE.RP.API.Models.LocalAuthority;
using HSE.RP.API.Extensions;
using HSE.RP.Domain.DynamicsDefinitions;
using HSE.RP.API.Models.Payment;
using HSEPortal.API.Models.Payment;

namespace HSE.RP.API.UnitTests
{
    public abstract class UnitTestBase
    {
        protected HttpTest HttpTest { get; }
        protected IDynamicsService DynamicsService { get; set; }
        protected DynamicsApi DynamicsApi { get; }
        protected OTPService OtpService { get; set; }
        protected IOptions<FeatureOptions> FeatureOptions = new OptionsWrapper<FeatureOptions>(new FeatureOptions());
        protected IOptions<IntegrationsOptions> IntegrationsOptions = new OptionsWrapper<IntegrationsOptions>(new IntegrationsOptions()
        {
            CommonAPIEndpoint = "http://localhost:7126",
            CommonAPIKey = "SY2hV4HWkDS-4eWOrND7KeY2Xtdi__ZtidERvbn4myUfAzFukDwEDg==",
            NotificationServiceApiKey = "buildingprofessionals-746d43a8-7ecc-49b5-b11c-f9d412865147-b3f01055-548e-4f55-997d-c3f9461d4480",
            NotificationsAPIEndpoint = "https://api.notifications.service.gov.uk"
        });
        protected IOptions<SwaOptions> swaOptions = new OptionsWrapper<SwaOptions>(new SwaOptions());

        protected readonly DynamicsOptions DynamicsOptions = new()
        {
            EnvironmentUrl = "https://bsr-ws2-dev.crm11.dynamics.com",
            TenantId = "6b5953be-6b1d-4980-b26b-56ed8b0bf3dc",
            ClientId = "bd44f58c-fa1a-4f58-a848-b24b3ef72719",
            ClientSecret = "tTY8Q~mdGiR1JsoMNQSBIzlKQoPcPr.eHzB_5crC",
            EmailVerificationFlowUrl = "http://flow_url",
            LocalAuthorityTypeId = "db305f3e-1dad-ed11-83ff-0022481b5e4f"
        };

        protected readonly IntegrationsOptions integrationOptions = new()
        {
            CommonAPIEndpoint= "http://localhost:7126",
            CommonAPIKey= "SY2hV4HWkDS-4eWOrND7KeY2Xtdi__ZtidERvbn4myUfAzFukDwEDg==",
            NotificationServiceApiKey= "buildingprofessionals-746d43a8-7ecc-49b5-b11c-f9d412865147-b3f01055-548e-4f55-997d-c3f9461d4480",
            NotificationsAPIEndpoint= "https://api.notifications.service.gov.uk/v2/notifications/email",
        };

        protected readonly FeatureOptions featureOptions = new()
        {
            DisableApplicationDuplicationCheck = false,
            DisableOtpValidation = false
        };

        protected EncodedRequest Base64Encode(Object plainObject)
        {
            var plainText = JsonSerializer.Serialize(plainObject);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return new EncodedRequest("base64:" + System.Convert.ToBase64String(plainTextBytes));
        }

        protected UnitTestBase()
        {
            FlurlHttp.Configure(settings => { settings.JsonSerializer = new SystemTextJsonSerializer(); });

            var options = new Mock<IOptions<DynamicsOptions>>();
            options.SetupGet(x => x.Value).Returns(DynamicsOptions);

            /*public DynamicsService(DynamicsModelDefinitionFactory dynamicsModelDefinitionFactory, IOptions<FeatureOptions> featureOptions, IOptions<DynamicsOptions> dynamicsOptions, IOptions<SwaOptions> swaOptions, DynamicsApi dynamicsApi)
            {
                this.dynamicsModelDefinitionFactory = dynamicsModelDefinitionFactory;
                this.dynamicsApi = dynamicsApi;
                this.swaOptions = swaOptions.Value;
                this.dynamicsOptions = dynamicsOptions.Value;
                this.featureOptions = featureOptions.Value;
            }*/

            HttpTest = new HttpTest();
            DynamicsApi = new DynamicsApi(options.Object);
            DynamicsService = new DynamicsService(new DynamicsModelDefinitionFactory(), new OptionsWrapper<FeatureOptions>(featureOptions), options.Object, new OptionsWrapper<SwaOptions>(new SwaOptions()), new DynamicsApi(options.Object));
            OtpService = new OTPService(IntegrationsOptions);
        }
        protected HttpRequestData BuildHttpRequestData<T>(T data, params string[] parameters)
        {
            return BuildHttpRequestDataWithUri(data, new Uri(DynamicsOptions.EnvironmentUrl));
        }

        protected HttpRequestData BuildHttpRequestData<T>(T data, Parameter[] parameters)
        {
            Uri uri = new Uri(DynamicsOptions.EnvironmentUrl).SetQueryParams(parameters.Select(p => new { key = p.Key, value = p.Value })).ToUri();
            return BuildHttpRequestDataWithUri(data, uri);
        }

        protected TestableHttpRequestData BuildHttpRequestDataWithUri<T>(T data, Uri uri)
        {
            var functionContext = new Mock<FunctionContext>();

            var memoryStream = new MemoryStream();
            JsonSerializer.Serialize(memoryStream, data);

            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new TestableHttpRequestData(functionContext.Object, uri, memoryStream);
        }

        protected object BuildODataEntityHeader(string id)
        {
            return $"OData-EntityId={DynamicsOptions.EnvironmentUrl}/api/data/v9.2/whatever_entity({id})";
        }
        protected IMapper GetMapper()
        {
            return new MapperConfiguration(config =>
            {
                config.AddProfile<OrdnanceSurveyPostcodeResponseProfile>();
                config.AddProfile<LocalAuthoritiesSearchResponseProfile>();
                config.AddProfile<CompaniesHouseSearchResponseProfile>();
                config.AddProfile<PaymentProfile>();
            }).CreateMapper();
        }

    }
}
public class Parameter
{
    public string Key { get; set; }
    public string Value { get; set; }
}