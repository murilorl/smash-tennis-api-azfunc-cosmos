using App.Data;
using App.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public class RankingsService : IRankingsService
    {
        private static readonly string ContainerName = "rankings";

        private readonly ICosmosService _cosmosService;
        private readonly Container _container;

        public RankingsService(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
            _container = _cosmosService.GetDatabase().GetContainer(ContainerName);
        }
        public Task<Ranking> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Ranking>> GetAllAsync(string queryString)
        {
            List<Ranking> rankings = new List<Ranking>();

            //QueryDefinition queryDefinition = new QueryDefinition("select * from ToDos t where t.cost > @expensive").WithParameter("@expensive", 9000);
            QueryDefinition queryDefinition = null;
            FeedIterator<Ranking> resultset = this._container.GetItemQueryIterator<Ranking>(
                queryDefinition,
                null,
                new QueryRequestOptions() { PartitionKey = new PartitionKey(2020) }
            );

            while (resultset.HasMoreResults)
            {
                //foreach (var item in await resultset.ReadNextAsync())
                //{
                //    rankings.Add(item);
                //}
                FeedResponse<Ranking> response = await resultset.ReadNextAsync();
                rankings.Add(response.First());
            }
            return rankings;
        }
    }
}

