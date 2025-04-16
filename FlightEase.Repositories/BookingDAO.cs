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
    public class BookingDAO : IDAO<Booking>
    {
        private readonly FlightDbContext _context;
        public BookingDAO(FlightDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking entity)
        {
            try
            {
                await _context.Bookings.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding ticket");
                throw ex;
            }
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
            try
            {
                return await _context.Bookings.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Dao");
                throw ex;
            }
        }

        public Task UpdateAsync(Booking entity)
        {
            throw new NotImplementedException();
        }
    }
}
