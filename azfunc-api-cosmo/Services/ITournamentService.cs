using App.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public interface ITournamentService
    {
        Task<Tournament> GetById(Guid id);
        Task<IEnumerable<Tournament>> GetAllAsync(string queryString);
        Task<Tournament> AddAsync(Tournament tournament);
    }
}