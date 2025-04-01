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
    public class ClassTypeService : IService<ClassType>
    {
        private readonly IDAO<ClassType> _dao;

        public ClassTypeService(IDAO<ClassType> dao)
        {
            _dao = dao;
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
            return await _dao.GetAllAsync();
        }

        public Task UpdateAsync(ClassType entity)
        {
            throw new NotImplementedException();
        }
    }
}
