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
    public class MealService : IService<Meal>
    {
        private readonly IDAO<Meal> _dao;

        public MealService(IDAO<Meal> dao)
        {
            _dao = dao;
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
            return await _dao.GetAllAsync();
        }

        public Task UpdateAsync(Meal entity)
        {
            throw new NotImplementedException();
        }
    }
}
