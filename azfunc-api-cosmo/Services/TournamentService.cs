using App.Data;
using App.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public class TournamentService : ITournamentService
    {
        private static readonly string ContainerName = "tournaments";

        private readonly ICosmosService _cosmosService;
        private readonly Container _container;

        public TournamentService(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
            _container = _cosmosService.GetDatabase().GetContainer(ContainerName);
        }
        Task<Tournament> ITournamentService.GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Tournament>> ITournamentService.GetAllAsync(string queryString)
        {
            throw new NotImplementedException();
        }

        public async Task<Tournament> AddAsync(Tournament tournament)
        {
            tournament.Id = Guid.NewGuid();
            tournament.Created = DateTime.Now;
            tournament.Updated = DateTime.Now;

            return await this._container.CreateItemAsync<Tournament>(
                tournament,
                new PartitionKey(tournament.Id.ToString()));
        }
    }
}

