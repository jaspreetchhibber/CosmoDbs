using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Fluent;
using System.Linq;
using PlantApp.Core.Photos;

namespace PlantApp.Core.CosmoDbServices
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }
        public async Task<string> AddItemAsync(Photo item)
        {
            try
            {
                var result = await this._container.CreateItemAsync<Photo>(item, new PartitionKey(item.Id));
                return "Success";
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Photo>(id, new PartitionKey(id));
        }

        public async Task<Photo> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Photo> response = await this._container.ReadItemAsync<Photo>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }
        public async Task<IEnumerable<Photo>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Photo>(new QueryDefinition(queryString));
            List<Photo> results = new List<Photo>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
