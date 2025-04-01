using AutoMapper;
using FlightEase.Domains.Entities;
using FlightEase.Services.Interfaces;
using FlightEase.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FlightEase.Controllers
{
    public class FlightController : Controller
    {

        private readonly IService<Flight> _flightService;
        private readonly IMapper _mapper;

        public FlightController(IService<Flight> flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var lstFlights = await _flightService.GetAllAsync();
                List<FlightVM>? flightVMs = null;

                if (lstFlights != null)
                {
                    flightVMs = _mapper.Map<List<FlightVM>>(lstFlights);
                    return View(flightVMs);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }
    }
}
