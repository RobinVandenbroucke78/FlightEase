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
    public class MealDAO : IDAO<Meal>
    {
        private readonly FlightDbContext _context;

        public MealDAO(FlightDbContext context)
        {
            _context = context;
        }
        public Task AddAsync(Meal entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Meal entity)
        {
            throw new NotImplementedException();
        }

        public Task<Meal?> FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Meal>?> GetAllAsync()
        {
            return await _context.Meals.ToListAsync();
        }

        public Task UpdateAsync(Meal entity)
        {
            throw new NotImplementedException();
        }
    }
}
