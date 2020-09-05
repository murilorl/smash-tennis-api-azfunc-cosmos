using App.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Azure.Cosmos;
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
        public async Task<App.Model.User> GetById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The Id cannot be null");
            }

            try
            {
                /*QueryDefinition query = new QueryDefinition("select * from u where t.id = @id and u.Deleted == false")
                     .WithParameter("@id", id.ToString());*/

                ItemResponse<App.Model.User> response = await this._container.ReadItemAsync<App.Model.User>(id.ToString(), new PartitionKey(id.ToString()));
                if (response.Resource.Deleted)
                    return null;

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }


        public async Task<IEnumerable<Model.User>> GetAllAsync(string queryString)
        {
            //var query = this._container.GetItemQueryIterator<Models.Users.User>(new QueryDefinition(queryString));
            FeedIterator<App.Model.User> feedIterator = this._container.GetItemQueryIterator<Model.User>(new QueryDefinition("SELECT * FROM c"));
            List<App.Model.User> results = new List<App.Model.User>();
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<Model.User> response = await feedIterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Model.User> AddAsync(Model.User user)
        {
            return await this._container.CreateItemAsync(user, new PartitionKey(user.Email));
        }

        public async Task<Model.User> UpdateAsync(Guid id, Model.User user)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }


            // Container.UpsertItemAsync rquires a JSON serializable object that must contain an id property
            user.Id = id;

            ItemResponse<Model.User> response;
            try
            {
                response = await this._container.ReplaceItemAsync(
                    user,
                    user.Id.ToString(),
                    new PartitionKey(user.Email)
                    );
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ArgumentException("Usuário não encontrado");
            }
            return response.Resource;
        }

        public Task UpdatePartialAsync(Guid guid, JsonPatchDocument<Model.User> patchUser)
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

            Model.User user = await GetById(id);
            user.Updated = DateTime.Now;
            user.Deleted = true;

            await this._container.UpsertItemAsync<App.Model.User>(user, new PartitionKey(id.ToString()));
            //await this._container.DeleteItemAsync<Models.Users.User>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<Model.User> GetByFacebookId(string facebookId, string email)
        {
            Model.User returnValue = null;

            if (String.IsNullOrWhiteSpace(facebookId))
            {
                throw new ArgumentNullException("The Facebook Id cannot be null");
            }

            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException("The email cannot be null Id cannot be null");
            }

            QueryDefinition queryDefinition = new QueryDefinition("select * from users u where u.facebookProfile.id = @facebookId")
                .WithParameter("@facebookId", facebookId);

            FeedIterator<App.Model.User> feedIterator = this._container.GetItemQueryIterator<Model.User>(
                queryDefinition,
                null,
                new QueryRequestOptions() { PartitionKey = new PartitionKey(email) }
            );

            while (feedIterator.HasMoreResults)
            {
                foreach (var user in await feedIterator.ReadNextAsync())
                {
                    returnValue = user;
                }
            }
            return returnValue;
        }
    }
}
