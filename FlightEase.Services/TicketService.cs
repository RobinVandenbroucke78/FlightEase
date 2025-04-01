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
    public class TicketService : IService<Ticket>
    {
        private readonly IDAO<Ticket> _dao;

        public TicketService(IDAO<Ticket> dao)
        {
            _dao = dao;
        }
        public Task AddAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ticket>?> GetAllAsync()
        {
            return await _dao.GetAllAsync();
        }

        public Task UpdateAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
