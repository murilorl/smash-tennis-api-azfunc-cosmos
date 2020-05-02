using App.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync(string queryString);
        Task<Models.Users.User> AddItemAsync(User user);
    }
}