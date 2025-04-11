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
    public class BookingService : IService<Booking>
    {
        private readonly IDAO<Booking> _dao;

        public BookingService(IDAO<Booking> dao)
        {
            _dao = dao;
        }

        public Task AddAsync(Booking entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Booking entity)
        {
            throw new NotImplementedException();
        }

        public Task<Booking?> FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Booking>?> GetAllAsync()
        {
            return await _dao.GetAllAsync();    
        }

        public Task UpdateAsync(Booking entity)
        {
            throw new NotImplementedException();
        }
    }
}
