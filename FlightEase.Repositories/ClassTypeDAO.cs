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
    public class ClassTypeDAO : IDAO<ClassType>
    {
        private readonly FlightDbContext _context;

        public ClassTypeDAO(FlightDbContext context)
        {
            _context = context;
        }
        public Task AddAsync(ClassType entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ClassType entity)
        {
            throw new NotImplementedException();
        }

        public Task<ClassType?> FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClassType>?> GetAllAsync()
        {
            return await _context.ClassTypes.ToListAsync();
        }

        public Task UpdateAsync(ClassType entity)
        {
            throw new NotImplementedException();
        }
    }
}
