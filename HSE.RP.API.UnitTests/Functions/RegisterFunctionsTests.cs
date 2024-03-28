using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Flurl;
using HSE.RP.API.Extensions;
using HSE.RP.API.Functions;
using HSE.RP.API.Models.Register;
using HSE.RP.API.Models.Search;
using HSE.RP.API.Services;
using HSE.RP.API.UnitTests.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace HSE.RP.API.UnitTests.Functions
{
    public class RegisterFunctionsTests
    {
        private readonly Mock<ICosmosDbService> cosmosDbServiceMock;
        private readonly Mock<IRegisterSearchService> registerSearchServiceMock;
        private readonly RegisterFunctions registerFunctions;

        public TestableHttpRequestData BuildHttpRequestDataWithUri<T>(T data, Uri uri)
        {
            var functionContext = new Mock<FunctionContext>();

            var memoryStream = new MemoryStream();
            JsonSerializer.Serialize(memoryStream, data);

            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new TestableHttpRequestData(functionContext.Object, uri, memoryStream);
        }

        public RegisterFunctionsTests()
        {
            cosmosDbServiceMock = new Mock<ICosmosDbService>();
            registerSearchServiceMock = new Mock<IRegisterSearchService>();
            registerFunctions = new RegisterFunctions(cosmosDbServiceMock.Object, registerSearchServiceMock.Object);
        }



        [Fact]
        public async Task SearchRBIInspectorNames_Should_ReturnBadRequest_When_CountryOrNameIsMissing()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com"));
            //requestMock.Setup(r => r.GetQueryParameters()).Returns(new Dictionary<string, StringValues>());

            // Act
            var response = await registerFunctions.SearchRBIInspectorNames(requestData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SearchRBIInspectorNames_Should_ReturnNotFound_When_NoInspectorNamesFound()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?name=John&country=England"));


            
            registerSearchServiceMock.Setup(s => s.GetRBIInspectorsByNameAndCountry("John", "England")).ReturnsAsync(new RBIInspectorNamesResponse { Inspectors = new List<string>(),Results=0 });

            // Act
            var response = await registerFunctions.SearchRBIInspectorNames(requestData);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SearchRBIInspectorNames_Should_ReturnInspectorNames_When_InspectorNamesFound()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?name=John&country=England"));

            registerSearchServiceMock.Setup(s => s.GetRBIInspectorsByNameAndCountry("John", "England"))
                .ReturnsAsync(new RBIInspectorNamesResponse { Inspectors = new List<string> { "John Doe", "John Smith" }, Results=2 });

            // Act
            var response = await registerFunctions.SearchRBIInspectorNames(requestData);
            var responseBody = await response.ReadAsJsonAsync<RBIInspectorNamesResponse>() ?? null;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Body);
            Assert.Equal(2, responseBody.Results);

        }

        [Fact]
        public async Task SearchRBICompanyNames_WithValidParameters_ReturnsCompanyNames()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?company=company&country=England"));


            var companyNames = new RBIInspectorCompaniesResponse
            {
                Companies = new List<string> { "Company 1", "Company 2" }, Results=2
            };
            registerSearchServiceMock.Setup(s => s.GetRBICompaniesByNameAndCountry("company", "England")).ReturnsAsync(companyNames);

            // Act
            var response = await registerFunctions.SearchRBICompanyNames(requestData);
            var responseBody = await response.ReadAsJsonAsync<RBIInspectorCompaniesResponse>() ?? null;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, responseBody.Results);
            Assert.Equal(2, responseBody.Companies.Count);
        }

        [Fact]
        public async Task SearchRBICompanyNames_WithNoCompanies_ReturnsNotFound()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?company=acme&country=England"));


            var companyNames = new RBIInspectorCompaniesResponse
            {
                Companies = new List<string>(), Results=0
            };
            registerSearchServiceMock.Setup(s => s.GetRBICompaniesByNameAndCountry("acme", "England")).ReturnsAsync(companyNames);

            // Act
            var response = await registerFunctions.SearchRBICompanyNames(requestData);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SearchRBICompanyNames_WithMissingParameters_ReturnsBadRequest()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?name=company&country=England"));

            // Act
            var response = await registerFunctions.SearchRBICompanyNames(requestData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task GetRegisterLastUpdated_ReturnsBadRequest_WhenCountryOrServiceIsNullOrWhiteSpace()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?country=England&service="));

            var registerFunctions = new RegisterFunctions(cosmosDbServiceMock.Object, registerSearchServiceMock.Object);

            // Act
            var response = await registerFunctions.GetRegisterLastUpdated(requestData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetRegisterLastUpdated_ReturnsObjectResponse_WhenCountryAndServiceAreValid()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?country=England&service=BuildingInspector"));

            var lastUpdateDate = "2022-01-01" ;
            registerSearchServiceMock.Setup(s => s.GetRegisterLastUpdated("BuildingInspector", "England")).ReturnsAsync(lastUpdateDate);

            var registerFunctions = new RegisterFunctions(cosmosDbServiceMock.Object, registerSearchServiceMock.Object);

            // Act
            var response = await registerFunctions.GetRegisterLastUpdated(requestData);
            var responseBody = await response.ReadAsJsonAsync<string>() ?? null;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(lastUpdateDate, responseBody);
        }

        [Fact]
        public async Task SearchRBIRegister_WithValidParameters_ReturnsSearchResults()
        {
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?country=England&name=john&company=corp"));

            // Arrange

            var searchResults = new RBISearchResponse
            {
                RBIApplications = new List<BuildingProfessionApplication>
                {
                    new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), ApplicationNumber = "123", BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "John Doe" }, Employer = new Employer{ EmployerName = "Ultra Corp", EmployerAddress = "1 Evilstreet" } },
                    new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), ApplicationNumber = "456", BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "John Smith" }, Employer = new Employer{ EmployerName = "Mega Corp", EmployerAddress = "Imaginary Way" } }
                },
                Results=2
            };
            registerSearchServiceMock.Setup(s => s.SearchRBIRegister("john", "corp", "England")).ReturnsAsync(searchResults);

            // Act
            var response = await registerFunctions.SearchRBIRegister(requestData);
            var responseBody = await response.ReadAsJsonAsync<RBISearchResponse>() ?? null;

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            registerSearchServiceMock.Verify(s => s.SearchRBIRegister("john", "corp", "England"), Times.Once);
            Assert.Equal(2, responseBody.Results);
            Assert.Equal(2, responseBody.RBIApplications.Count);
        }

        [Fact]
        public async Task SearchRBIRegister_WithInvalidCountry_ReturnsBadRequest()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?country=&name=john&company=corp"));


            // Act
            var response = await registerFunctions.SearchRBIRegister(requestData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            registerSearchServiceMock.Verify(s => s.SearchRBIRegister(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task SearchRBIRegister_WithNoNameAndCompany_ReturnsBadRequest()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?country=England&name=&company="));


            // Act
            var response = await registerFunctions.SearchRBIRegister(requestData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            registerSearchServiceMock.Verify(s => s.SearchRBIRegister(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task SearchRBIRegister_WithNoSearchResults_ReturnsNotFound()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com?country=England&name=john&company=corp"));

            var searchResults = new RBISearchResponse { RBIApplications = new List<BuildingProfessionApplication>(), Results=0 };
            registerSearchServiceMock.Setup(s => s.SearchRBIRegister("john", "corp", "England")).ReturnsAsync(searchResults);

            // Act
            var response = await registerFunctions.SearchRBIRegister(requestData);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            registerSearchServiceMock.Verify(s => s.SearchRBIRegister("john", "corp", "England"), Times.Once);
        }


        [Fact]
        public async Task GetRBIDetails_ValidId_ReturnsOkResponse()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com/123"));
            var id = "123";
            var rbiDetails = new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), ApplicationNumber = "123", BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "John Doe" }, Employer = new Employer { EmployerName = "Ultra Corp", EmployerAddress = "1 Evilstreet" } };


            registerSearchServiceMock.Setup(x => x.GetRBIDetails(id)).ReturnsAsync(rbiDetails);

            // Act
            var response = await registerFunctions.GetRBIDetails(requestData, id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetRBIDetails_InvalidId_ReturnsBadRequestResponse()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com/123"));
            string id = null;

            // Act
            var response = await registerFunctions.GetRBIDetails(requestData, id);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetRBIDetails_NullDetails_ReturnsNotFoundResponse()
        {
            // Arrange
            var requestData = BuildHttpRequestDataWithUri<string>("test", new Uri("http://noaddress.com/123"));
            var id = "123";


            registerSearchServiceMock.Setup(x => x.GetRBIDetails(id)).ReturnsAsync((BuildingProfessionApplication)null);

            // Act
            var response = await registerFunctions.GetRBIDetails(requestData, id);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


    }

}

