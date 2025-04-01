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
     public class SeatService : IService<Seat>
     {
        private readonly IDAO<Seat> _dao;

        public SeatService(IDAO<Seat> dao)
        {
            _dao = dao;
        }
        public Task AddAsync(Seat entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Seat entity)
        {
            throw new NotImplementedException();
        }

        public Task<Seat?> FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Seat>?> GetAllAsync()
        {
            return await _dao.GetAllAsync();
        }

        public Task UpdateAsync(Seat entity)
        {
            throw new NotImplementedException();
        }
    }
}
