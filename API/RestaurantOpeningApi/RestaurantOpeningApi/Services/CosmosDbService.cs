using Microsoft.Azure.Cosmos;

namespace RestaurantOpeningApi.Services
{
    public class CosmosDbService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseId;
        private readonly string _containerId;

        public CosmosDbService(IConfiguration configuration)
        {
            var cosmosDbSettings = configuration.GetSection("CosmosDb").Get<CosmosDbSettings>();
            _cosmosClient = new CosmosClient(cosmosDbSettings.AccountEndpoint, cosmosDbSettings.AccountKey);
            _databaseId = cosmosDbSettings.DatabaseId;
            _containerId = cosmosDbSettings.ContainerId;
        }
    }

    public class CosmosDbSettings
    {
        public string AccountEndpoint { get; set;}
        public string AccountKey { get; set;}
        public string DatabaseId { get; set;}
        public string ContainerId { get; set;}
    }
}
