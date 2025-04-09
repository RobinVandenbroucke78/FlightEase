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
    public class SeasonDAO : IDAO<Season>
    {
        private readonly FlightDbContext _context;

        public SeasonDAO(FlightDbContext context)
        {
            _context = context;
        }
        public Task AddAsync(Season entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Season entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Season?> FindByIdAsync(int Id)
        {
            try
            {
                return await _context.Seasons
                    .Where(c => c.SeasonId == Id)
                    .FirstOrDefaultAsync(c => c.SeasonId == Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Dao");
                throw ex;
            }
        }

        public async Task<IEnumerable<Season>?> GetAllAsync()
        {
            return await _context.Seasons.ToListAsync();
        }

        public Task UpdateAsync(Season entity)
        {
            throw new NotImplementedException();
        }
    }
}
