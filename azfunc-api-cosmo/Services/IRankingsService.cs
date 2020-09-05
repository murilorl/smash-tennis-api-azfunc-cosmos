using App.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IRankingsService
    {
        Task<Ranking> GetById(Guid id);
        Task<IEnumerable<Ranking>> GetAllAsync(string queryString);
    }
}