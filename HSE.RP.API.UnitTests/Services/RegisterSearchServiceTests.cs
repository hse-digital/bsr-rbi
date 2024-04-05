using HSE.RP.API.Models.Register;
using HSE.RP.API.Models.Search;
using HSE.RP.API.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HSE.RP.API.UnitTests.Services
{
    public class RegisterSearchServiceTests
    {
        private readonly Mock<ICosmosDbService> cosmosDbServiceMock;
        private readonly IRegisterSearchService registerSearchService;

        public RegisterSearchServiceTests()
        {
            cosmosDbServiceMock = new Mock<ICosmosDbService>();
            registerSearchService = new RegisterSearchService(cosmosDbServiceMock.Object);
        }

        [Fact]
        public async Task GetRBIInspectorsByNameAndCountry_ShouldReturnInspectorNames()
        {
            // Arrange
            var name = "John Doe";
            var country = "England";
            var nameResponse = new List<BuildingProfessionApplication>
            {
                    new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "John Doe" } },
                    new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "Jane Smith" } }
            };
            cosmosDbServiceMock.Setup(x => x.GetInspectorsByNameAndCountry(name, country)).ReturnsAsync(nameResponse);

            // Act
            var result = await registerSearchService.GetRBIInspectorsByNameAndCountry(name, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results);
            Assert.Equal(new List<string> { "John Doe", "Jane Smith" }, result.Inspectors);
        }

        [Fact]
        public async Task GetRBICompaniesByNameAndCountry_ShouldReturnCompanyNames()
        {
            // Arrange
            var company = "ABC";
            var country = "England";
            var companyResponse = new List<BuildingProfessionApplication>
            {
                    new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "John Doe" }, Employer = new Employer{ EmployerName = "ABC Corp", EmployerAddress = "1 Evilstreet" } },
                    new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "Jane Smith" }, Employer = new Employer{ EmployerName = "ABCDEFG", EmployerAddress = "Imaginary Way" } }
            };
            cosmosDbServiceMock.Setup(x => x.GetInspectorsByCompanyAndCountry(company, country)).ReturnsAsync(companyResponse);

            // Act
            var result = await registerSearchService.GetRBICompaniesByNameAndCountry(company, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results);
            Assert.Equal(new List<string> { "ABC Corp", "ABCDEFG" }, result.Companies);
        }

        [Fact]
        public async Task GetRegisterLastUpdated_ShouldReturnLastUpdatedDate()
        {
            // Arrange
            var service = "BuildingInspector";
            var country = "England";

            var lastUpdated = new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = new DateTime(2022, 1, 1), Applicant = new Applicant { ApplicantName = "John Doe" } };
            cosmosDbServiceMock.Setup(x => x.GetLastUpdatedByBuildingProfessionTypeAndCountry(service, country)).ReturnsAsync(lastUpdated);

            // Act
            var result = await registerSearchService.GetRegisterLastUpdated(service, country);

            // Assert
            Assert.Equal("1 January 2022", result);
        }

        [Fact]
        public async Task SearchRBIRegister_ShouldReturnSearchResults()
        {
            // Arrange
            var name = "John Doe";
            var company = "ABC Corp";
            var country = "England";
            var searchResponse = new List<BuildingProfessionApplication>
            {
                    new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "John Doe" } },
                    new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "Jane Smith" } }
            };
            cosmosDbServiceMock.Setup(x => x.SearchRBIRegister(name, company, country)).ReturnsAsync(searchResponse);

            // Act
            var result = await registerSearchService.SearchRBIRegister(name, company, country);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results);
            Assert.Equal(searchResponse.OrderBy(a => a.Applicant.ApplicantName).ToList(), result.RBIApplications);
        }


        [Fact]
        public async Task GetRBIDetails_ShouldReturnRBIDetails()
        {
            // Arrange
            var id = "123";
            var rbiDetails = new BuildingProfessionApplication { Id = Guid.NewGuid().ToString(), BuildingProfessionType = "BuildingInspector", Countries = new List<string> { "England" }, CreationDate = DateTime.Now, Applicant = new Applicant { ApplicantName = "John Doe" } };
            cosmosDbServiceMock.Setup(x => x.GetItemAsync<BuildingProfessionApplication>(id, "BuildingInspector")).ReturnsAsync(rbiDetails);

            // Act
            var result = await registerSearchService.GetRBIDetails(id);

            // Assert
            Assert.Equal(rbiDetails, result);
        }

    }
}
