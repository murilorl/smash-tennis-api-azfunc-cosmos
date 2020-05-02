using App.Settings;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace App.Data
{
    public class CosmosService : ICosmosService
    {

        private readonly CosmosDbSettings _cosmosDbSettings;

        private CosmosClient Client;
        private Database Database;

        public CosmosService(IOptions<CosmosDbSettings> cosmosDbSettings)
        {
            _cosmosDbSettings = cosmosDbSettings.Value;
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(_cosmosDbSettings.Account, _cosmosDbSettings.Key);
            Client = clientBuilder
                .WithConnectionModeDirect()
                .Build();

            CreateDatabaseIfNotExistsAsync(_cosmosDbSettings.DatabaseId).Wait();
        }

        private async Task CreateDatabaseIfNotExistsAsync(string databaseId)
        {
            DatabaseResponse databaseResponse = await this.Client.CreateDatabaseIfNotExistsAsync(databaseId);
            this.Database = databaseResponse.Database;
        }

        public Database GetDatabase()
        {
            return this.Database;
        }
    }
}
