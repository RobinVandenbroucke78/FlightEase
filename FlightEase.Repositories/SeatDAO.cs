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
    public class SeatDAO : IDAO<Seat>
    {
        private readonly FlightDbContext _context;

        public SeatDAO(FlightDbContext context)
        {
            _context = context;
        }
        public Task AddAsync(Seat entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Seat entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Seat?> FindByIdAsync(int Id)
        {
            try
            {
                return await _context.Seats
                    .Where(s => s.SeatId == Id)
                    .FirstOrDefaultAsync(s => s.SeatId == Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Dao");
                throw ex;
            }
        }

        public async Task<IEnumerable<Seat>?> GetAllAsync()
        {
            return await _context.Seats.ToListAsync();
        }

        public Task UpdateAsync(Seat entity)
        {
            throw new NotImplementedException();
        }
    }
}
