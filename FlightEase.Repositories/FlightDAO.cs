﻿using FlightEase.Domains.Data;
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

        public async Task<Flight?> FindByIdAsync(int Id)
        {
            try
            {
                return await _context.Flights
                    .Where(f => f.FlightId == Id)
                    .Include(f => f.FromAirport).ThenInclude(a => a.City)
                    .Include(f => f.ToAirport).ThenInclude(a => a.City)
                    .Include(f => f.Transfer).ThenInclude(t => t.FirstAirport).ThenInclude(a => a.City)
                    .Include(f => f.Transfer).ThenInclude(t => t.SecondAirport).ThenInclude(a => a.City)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DAO");
                throw ex;
            }
        }

        public async Task<IEnumerable<Flight>?> GetAllAsync()
        {
            try
            {
               return await _context.Flights
                    .Include(f => f.FromAirport).ThenInclude(a => a.City)
                    .Include(f => f.ToAirport).ThenInclude(a => a.City)
                    .Include(f => f.Transfer).ThenInclude(t => t.FirstAirport).ThenInclude(a => a.City)
                    .Include(f => f.Transfer).ThenInclude(t => t.SecondAirport).ThenInclude(a => a.City)
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
