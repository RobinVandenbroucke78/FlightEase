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
    public class TicketDAO : IDAO<Ticket>
    {
        private readonly FlightDbContext _context;

        public TicketDAO(FlightDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Ticket entity)
        {
            try
            {
                await _context.Tickets.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding ticket");
                throw ex;
            }
        }

        public Task DeleteAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket?> FindByIdAsync(int Id)
        {
            try
            {
                return await _context.Tickets
                     .Where(t => t.TicketId == Id)
                     .Include(t => t.Meal)
                     .Include(t => t.Season)
                     .Include(t => t.ClassType)
                     .Include(t => t.Seat)
                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Dao");
                throw ex;
            }
        }

        public async Task<IEnumerable<Ticket>?> GetAllAsync()
        {
            try
            {
                return await _context.Tickets
                     .Include(t => t.Meal)
                     .Include(t => t.Season)
                     .Include(t => t.ClassType)
                     .Include(t => t.Seat)
                     .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Dao");
                throw ex;
            }
        }

        public Task UpdateAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
