using App.Data;
using Microsoft.AspNetCore.JsonPatch;
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

        public UserService(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
            _container = _cosmosService.GetDatabase().GetContainer(ContainerName);
        }

        // Query by Id. If the entity is marked as deleted, returns null.
        public async Task<Models.Users.User> GetById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The Id cannot be null");
            }

            try
            {
                /*QueryDefinition query = new QueryDefinition("select * from u where t.id = @id and u.Deleted == false")
                     .WithParameter("@id", id.ToString());*/

                ItemResponse<Models.Users.User> response = await this._container.ReadItemAsync<Models.Users.User>(id.ToString(), new PartitionKey(id.ToString()));
                if (response.Resource.Deleted)
                    return null;

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }


        public async Task<IEnumerable<Models.Users.User>> GetAllAsync(string queryString)
        {
            //var query = this._container.GetItemQueryIterator<Models.Users.User>(new QueryDefinition(queryString));
            FeedIterator<Models.Users.User> feedIterator = this._container.GetItemQueryIterator<Models.Users.User>(new QueryDefinition("SELECT * FROM c"));
            List<Models.Users.User> results = new List<Models.Users.User>();
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<Models.Users.User> response = await feedIterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Models.Users.User> AddAsync(Models.Users.User user)
        {
            user.Id = Guid.NewGuid();
            user.Created = DateTime.Now;
            user.Updated = DateTime.Now;
            user.Deleted = false;

            return await this._container.CreateItemAsync<Models.Users.User>(user, new PartitionKey(user.Id.ToString()));
        }

        public async Task UpdateAsync(Guid id, Models.Users.User user)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }

            // Container.UpsertItemAsync rquires a JSON serializable object that must contain an id property
            user.Id = id;
            user.Updated = DateTime.Now;
            try
            {
                ItemResponse<Models.Users.User> response = await this._container.UpsertItemAsync<Models.Users.User>(user, new PartitionKey(id.ToString()));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ArgumentException("Usuário não encontrado");
            }
        }

        public Task UpdatePartialAsync(Guid guid, JsonPatchDocument<Models.Users.User> patchUser)
        {
            throw new NotImplementedException();
        }

        //Performs a soft delete instead of a hard delete
        public async Task DeleteAsync(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }

            Models.Users.User user = await GetById(id);
            user.Updated = DateTime.Now;
            user.Deleted = true;

            await this._container.UpsertItemAsync<Models.Users.User>(user, new PartitionKey(id.ToString()));
            //await this._container.DeleteItemAsync<Models.Users.User>(id.ToString(), new PartitionKey(id.ToString()));
        }
    }
}
