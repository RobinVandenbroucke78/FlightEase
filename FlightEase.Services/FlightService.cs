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
    public class FlightService : IService<Flight>
    {
        private readonly IDAO<Flight> _dao;

        public FlightService(IDAO<Flight> dao)
        {
            _dao = dao;
        }

        public Task AddAsync(Flight entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Flight entity)
        {
            throw new NotImplementedException();
        }

        public Task<Flight?> FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Flight>?> GetAllAsync()
        {
            return await _dao.GetAllAsync();
        }

        public Task UpdateAsync(Flight entity)
        {
            throw new NotImplementedException();
        }
    }
}
