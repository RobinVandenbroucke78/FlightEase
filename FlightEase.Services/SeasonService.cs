using FlightEase.Domains.Entities;
using FlightEase.Repositories.Interfaces;
using FlightEase.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightEase.Services
{
    public class SeasonService : IService<Season>
    {
        private readonly IDAO<Season> _dao;

        public SeasonService(IDAO<Season> dao)
        {
            _dao = dao;
        }
        public Task AddAsync(Season entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Season entity)
        {
            throw new NotImplementedException();
        }

        public Task<Season?> FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Season>?> GetAllAsync()
        {
            return await _dao.GetAllAsync();
        }

        public Task UpdateAsync(Season entity)
        {
            throw new NotImplementedException();
        }
    }
}
