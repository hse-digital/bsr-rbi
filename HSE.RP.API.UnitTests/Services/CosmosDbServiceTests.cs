using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using HSE.RP.API.Models.Register;
using HSE.RP.API.Services;
using System.Net;
using HSE.RP.API.Models.Enums;

namespace HSE.RPR.API.UnitTests.Services
{
    public class CosmosDbServiceTests
    {
        private readonly Mock<CosmosClient> _cosmosClientMock;
        private readonly Mock<Database> _databaseMock;
        private readonly Mock<Container> _containerMock;
        private readonly Mock<IOptions<IntegrationsOptions>> _integrationsOptionsMock;
        private readonly CosmosDbService _cosmosDbService;

        public CosmosDbServiceTests()
        {
            _cosmosClientMock = new Mock<CosmosClient>();
            _databaseMock = new Mock<Database>();
            _containerMock = new Mock<Container>();
            _integrationsOptionsMock = new Mock<IOptions<IntegrationsOptions>>();

            //Create test TestItem
            var testItem = new TestItem
            {
                Id = "test-id",
                Name = "Test Item",
                BuildingProfessionType = "test-building-profession-type",
                CreationDate = DateTime.Now,
                ApplicationNumber = "test-application-number",
                Countries = new System.Collections.Generic.List<string> { "test-country-1", "test-country-2" },
                DecisionCondition = "test-decision-condition",
                DecisionConditionReason = "test-decision-condition-reason",
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now
            };

            var mockResponse = new Mock<ItemResponse<TestItem>>();
            mockResponse.Setup(x => x.Resource).Returns(testItem);
            mockResponse.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);

            _cosmosClientMock.Setup(c => c.GetDatabase(It.IsAny<string>())).Returns(_databaseMock.Object);
            _databaseMock.Setup(d => d.GetContainer(It.IsAny<string>())).Returns(_containerMock.Object);
            _containerMock.Setup(c => c.ReadItemAsync<TestItem>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default)).ReturnsAsync(mockResponse.Object);
            _containerMock.Setup(d => d.DeleteItemAsync<TestItem>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, default)).ReturnsAsync(mockResponse.Object);
            //Mock IQueryable<T> query = container.GetItemLinqQueryable<T>().Where(item => item.BuildingProfessionType == buildingProfessionType);



            _cosmosDbService = new CosmosDbService(_cosmosClientMock.Object, _databaseMock.Object, _containerMock.Object);

        }

        [Fact]
        public async Task CreateDatabaseAsync_ShouldCall_CosmosClient_CreateDatabaseIfNotExistsAsync()
        {
            // Arrange
            string databaseName = "test-database";

            // Act
            await _cosmosDbService.CreateDatabaseAsync(databaseName);

            // Assert
            _cosmosClientMock.Verify(c => c.CreateDatabaseIfNotExistsAsync(databaseName, It.IsAny<int?>(), null, default), Times.Once);
        }

        [Fact]
        public async Task CreateContainerAsync_ShouldCall_Database_CreateContainerIfNotExistsAsync()
        {
            // Arrange
            string databaseName = "test-database";
            string containerName = "test-container";
            string partitionKeyPath = "/partitionKey";

            // Act
            await _cosmosDbService.CreateContainerAsync(databaseName, containerName, partitionKeyPath);

            // Assert
            _databaseMock.Verify(d => d.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath, It.IsAny<int?>(),null,default), Times.Once);
        }

       [Fact]
        public async Task AddItemAsync_ShouldCall_Container_CreateItemAsync()
        {
            // Arrange
            var item = new TestItem { Id = "test-id", Name = "Test Item" };
            string partitionKeyValue = "test-partition-key";

            // Act
            await _cosmosDbService.AddItemAsync(item, partitionKeyValue);

            // Assert
            _containerMock.Verify(c => c.CreateItemAsync(item, new PartitionKey(partitionKeyValue), null, default), Times.Once);
        }

        
        [Fact]
        public async Task GetItemAsync_ShouldCall_Container_ReadItemAsync()
        {
            // Arrange
            string id = "test-id";
            string partitionKeyValue = "test-partition-key";

            // Act
            var item = await _cosmosDbService.GetItemAsync<TestItem>(id, partitionKeyValue);

            // Assert
            _containerMock.Verify(c => c.ReadItemAsync<TestItem>(id, new PartitionKey(partitionKeyValue), null, default), Times.Once);
        }


        public class TestItem : IBuildingProfessionApplication
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string BuildingProfessionType { get; set; }
            public DateTime CreationDate { get; set; }
            public string ApplicationNumber { get; set; }
            public List<string> Countries { get; set; }
            public string DecisionCondition { get; set; }
            public string DecisionConditionReason { get; set; }
            public DateTime ValidFrom { get; set; }
            public DateTime ValidTo { get; set; }
            public List<Activity> Activities { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public Applicant Applicant { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public List<string> Classes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public Employer Employer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }
    }
}
