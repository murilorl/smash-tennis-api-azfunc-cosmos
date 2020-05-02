using App.Data;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public class UserService : IUserService
    {
        private static readonly string ContainerName = "users";

        private readonly ICosmosService _cosmosService;
        private readonly Container _container;

        public UserService(ICosmosService cosmosService, IConfiguration configuration)
        {
            _cosmosService = cosmosService;
            _container = _cosmosService.GetDatabase().GetContainer(ContainerName);
        }

        public async Task<IEnumerable<Models.Users.User>> GetUsersAsync(string queryString)
        {
            //var query = this._container.GetItemQueryIterator<Models.Users.User>(new QueryDefinition(queryString));
            var query = this._container.GetItemQueryIterator<Models.Users.User>(new QueryDefinition("SELECT * FROM c"));
            List<Models.Users.User> results = new List<Models.Users.User>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Models.Users.User> AddItemAsync(Models.Users.User user)
        {
            user.Id = Guid.NewGuid();
            user.Created = DateTime.Now;
            user.Updated = DateTime.Now;
            user.Deleted = false;

            return await this._container.CreateItemAsync<Models.Users.User>(user, new PartitionKey(user.Id.ToString()));
        }
    }
}
