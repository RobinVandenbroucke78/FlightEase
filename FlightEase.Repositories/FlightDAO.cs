using FlightEase.Domains.Data;
using FlightEase.Domains.Entities;
using FlightEase.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightEase.Repositories
{
    public class FlightDAO : IDAO<Flight>
    {
        private readonly FlightDbContext _context;

        public FlightDAO(FlightDbContext context)
        {
            _context = context;
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
            try
            {
               return await _context.Flights
                    .Include(f => f.FromAirport)
                    .Include(f => f.ToAirport)
                    .Include(f => f.Transfer)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Dao");
                throw ex;
            }
        }

        public Task UpdateAsync(Flight entity)
        {
            throw new NotImplementedException();
        }
    }
}
