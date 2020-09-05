using App.Model;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IUserService
    {
        Task<User> GetById(Guid id);
        Task<IEnumerable<User>> GetAllAsync(string queryString);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(Guid id, User user);
        Task UpdatePartialAsync(Guid id, JsonPatchDocument<User> patchUser);
        Task DeleteAsync(Guid id);
        Task<User> GetByFacebookId(string facebookId, string email);
    }
}