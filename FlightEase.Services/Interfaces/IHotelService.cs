using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightEase.Domains;

namespace FlightEase.Services.Interfaces
{
    public interface IHotelService : IService<Hotel>
    {
        Task<List<Hotel>?> GetHotelsByCityAsync(string city);
    }
}
