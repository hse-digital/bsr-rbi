using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using HSE.RP.API.Models.Register;
using Microsoft.Azure.Cosmos.Linq;
using HSE.RP.API.Models.Enums;
using System.Runtime.ExceptionServices;

namespace HSE.RP.API.Services
{
    public interface ICosmosDbService
    {
        /// <summary>
        /// Adds an item to the Cosmos DB container.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item to add.</param>
        /// <param name="partitionKeyValue">The partition key value.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddItemAsync<T>(T item, string partitionKeyValue);

        /// <summary>
        /// Creates a container in the Cosmos DB database.
        /// </summary>
        /// <param name="databaseName">The name of the database.</param>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="partitionKeyPath">The partition key path.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateContainerAsync(string databaseName, string containerName, string partitionKeyPath);

        /// <summary>
        /// Creates a database in the Cosmos DB account.
        /// </summary>
        /// <param name="databaseName">The name of the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateDatabaseAsync(string databaseName);

        /// <summary>
        /// Gets an item from the Cosmos DB container.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="id">The ID of the item.</param>
        /// <param name="partitionKeyValue">The partition key value.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<T> GetItemAsync<T>(string id, string partitionKeyValue);

        /// <summary>
        /// Removes items from the Cosmos DB container by building profession type.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="buildingProfessionType">The building profession type.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveItemsByBuildingProfessionType<T>(string buildingProfessionType) where T : IBuildingProfessionApplication;

        /// <summary>
        /// Removes items from the Cosmos DB container by building profession type and creation date.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="buildingProfessionType">The building profession type.</param>
        /// <param name="creationDate">The creation date.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveItemsByBuildingProfessionTypeAndCreationDateAsync<T>(string buildingProfessionType, DateTime creationDate) where T : IBuildingProfessionApplication;

        /// <summary>
        /// Gets a list of inspectors from the Cosmos DB container by company and country.
        /// </summary>
        /// <param name="name">The name of the inspector.</param>
        /// <param name="country">The country of the inspector.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<List<BuildingProfessionApplication>?> GetInspectorsByCompanyAndCountry(string name, string country);


        /// <summary>
        /// Gets a list of inspectors from the Cosmos DB container by name and country.
        /// </summary>
        /// <param name="name">The name of the inspector.</param>
        /// <param name="country">The country of the inspector.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<List<BuildingProfessionApplication>?> GetInspectorsByNameAndCountry(string name, string country);

        /// <summary>
        /// Gets the last updated building profession application from the Cosmos DB container by building profession type and country.
        /// </summary>
        /// <param name="buildingProfessionType">The building profession type.</param>
        /// <param name="country">The country of the building profession application.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<BuildingProfessionApplication> GetLastUpdatedByBuildingProfessionTypeAndCountry(string buildingProfessionType, string country);

        /// <summary>
        /// Searches the RBI register in the Cosmos DB container by name, company, and country.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <param name="company">The company to search for.</param>
        /// <param name="country">The country to search for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<List<BuildingProfessionApplication>> SearchRBIRegister(string name, string company, string country);

    }

    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosClient cosmosClient;
        private readonly Database database;
        private readonly Container container;

        public CosmosDbService(CosmosClient cosmosClient, Database database, Container container)
        {
            this.cosmosClient = cosmosClient;
            this.database = database;
            this.container = container;
        }

        /// <inheritdoc/>
        public async Task CreateDatabaseAsync(string databaseName)
        {
            await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
        }

        /// <inheritdoc/>
        public async Task CreateContainerAsync(string databaseName, string containerName, string partitionKeyPath)
        {
            Database database = cosmosClient.GetDatabase(databaseName);
            await database.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath);
        }

        /// <inheritdoc/>
        public async Task AddItemAsync<T>(T item, string partitionKeyValue)
        {
            await container.CreateItemAsync(item, new PartitionKey(partitionKeyValue));
        }

        /// <inheritdoc/>
        public async Task<T> GetItemAsync<T>(string id, string partitionKeyValue)
        {
            try
            {

            ItemResponse<T> response = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKeyValue));
            return response.Resource;
        }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        /// <inheritdoc/>
        public async Task RemoveItemsByBuildingProfessionType<T>(string buildingProfessionType) where T : IBuildingProfessionApplication
        {
            IQueryable<T> query = container.GetItemLinqQueryable<T>()
                .Where(item => item.BuildingProfessionType == buildingProfessionType);

            FeedIterator<T> queryResultSetIterator = query.ToFeedIterator();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach (T item in currentResultSet)
                {
                    // Get the id property using reflection
                    string itemId = item.Id;

                    if (itemId != null)
                    {
                        // Delete the item using the Cosmos DB container
                        await container.DeleteItemAsync<T>(itemId, new PartitionKey(item.BuildingProfessionType));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task RemoveItemsByBuildingProfessionTypeAndCreationDateAsync<T>(string buildingProfessionType, DateTime creationDate) where T : IBuildingProfessionApplication
        {
            IQueryable<T> query = container.GetItemLinqQueryable<T>()
                .Where(item => item.BuildingProfessionType == buildingProfessionType && item.CreationDate <= creationDate);

            FeedIterator<T> queryResultSetIterator = query.ToFeedIterator();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach (T item in currentResultSet)
                {
                    // Get the id property using reflection
                    string itemId = item.Id;

                    if (itemId != null)
                    {
                        // Delete the item using the Cosmos DB container
                        await container.DeleteItemAsync<T>(itemId, new PartitionKey(item.BuildingProfessionType));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task<List<BuildingProfessionApplication>?> GetInspectorsByNameAndCountry(string name, string country)
        {
            var RBIApplcations = new List<BuildingProfessionApplication>();
            IQueryable<BuildingProfessionApplication> query = container.GetItemLinqQueryable<BuildingProfessionApplication>()
                .Where(item => item.Countries.Any(c => c.Contains(country) 
                 && item.Applicant.ApplicantName.ToLower().Contains(name.ToLower())
                 && item.BuildingProfessionType == BuildingProfessionType.BuildingInspector.ToString()));

            FeedIterator<BuildingProfessionApplication> queryResultSetIterator = query.ToFeedIterator();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<BuildingProfessionApplication> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                RBIApplcations.AddRange(currentResultSet);
            }

            return RBIApplcations;
        }

        /// <inheritdoc/>
        public async Task<List<BuildingProfessionApplication>?> GetInspectorsByCompanyAndCountry(string name, string country)
        {
            var RBIApplcations = new List<BuildingProfessionApplication>();
            IQueryable<BuildingProfessionApplication> query = container.GetItemLinqQueryable<BuildingProfessionApplication>()
                .Where(item => item.Countries.Any(c => c.Contains(country) && item.Employer.EmployerName.ToLower().Contains(name.ToLower())
                && item.BuildingProfessionType == BuildingProfessionType.BuildingInspector.ToString()));

            FeedIterator<BuildingProfessionApplication> queryResultSetIterator = query.ToFeedIterator();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<BuildingProfessionApplication> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                RBIApplcations.AddRange(currentResultSet);
            }

            return RBIApplcations;
        }



        /// <inheritdoc/>
        public async Task<BuildingProfessionApplication> GetLastUpdatedByBuildingProfessionTypeAndCountry(string buildingProfessionType, string country)
        {
            IQueryable<BuildingProfessionApplication> query = container.GetItemLinqQueryable<BuildingProfessionApplication>()
                .Where(item => item.BuildingProfessionType == buildingProfessionType && item.Countries.Any(c => c.Contains(country)))
                .Take(1);

            FeedIterator<BuildingProfessionApplication> queryResultSetIterator = query.ToFeedIterator();

            FeedResponse<BuildingProfessionApplication> response = await queryResultSetIterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<List<BuildingProfessionApplication>> SearchRBIRegister(string name, string company, string country)
        {
            var RBIApplcations = new List<BuildingProfessionApplication>();

            IQueryable<BuildingProfessionApplication> query = container.GetItemLinqQueryable<BuildingProfessionApplication>()
                .Where(
                    item => item.Countries.Any(c => c.Contains(country)
                    && item.BuildingProfessionType == "BuildingInspector"
                    && item.Applicant.ApplicantName.ToLower().Contains(name.ToLower())
                    && item.Employer.EmployerName.ToLower().Contains(company.ToLower())));

            FeedIterator<BuildingProfessionApplication> queryResultSetIterator = query.ToFeedIterator();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<BuildingProfessionApplication> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                RBIApplcations.AddRange(currentResultSet);
            }

            return RBIApplcations;
        }

    }

}
