using AutoMapper;
using FlightEase.Domains.Entities;
using FlightEase.Extentions;
using FlightEase.Services.Interfaces;
using FlightEase.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlightEase.Controllers
{
    public class FlightController : Controller
    {

        private readonly IService<Flight> _flightService;
        private readonly IService<ClassType> _classTypeService;
        private readonly IService<Meal> _mealService;
        private readonly IService<Season> _seasonService;
        private readonly IService<Seat> _seatService;
        private readonly IMapper _mapper;

        public FlightController(IService<Flight> flightService, IMapper mapper, IService<ClassType> classTypeService, IService<Meal> mealService, IService<Season> seasonService, IService<Seat> seatService)
        {
            _flightService = flightService;
            _mapper = mapper;
            _classTypeService = classTypeService;
            _mealService = mealService;
            _seasonService = seasonService;
            _seatService = seatService;
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

        //ShoppingCart
        public async Task<IActionResult> Select(int flightId)
        {
            var flight = await _flightService.FindByIdAsync(flightId);
            
            if (flight == null)
            {
                return NotFound();
            }

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM();
            shoppingCartVM.ClassTypes = new SelectList(
                await _classTypeService.GetAllAsync(),
                "ClassTypeId",
                "ClassName"
            );
            shoppingCartVM.Meals = new SelectList(
                await _mealService.GetAllAsync(),
                "MealId",
                "MealName"
            );
            shoppingCartVM.Seasons = new SelectList(
                await _seasonService.GetAllAsync(),
                "SeasonId",
                "SeasonName"
            );
            shoppingCartVM.Seats = new SelectList(
                await _seatService.GetAllAsync(),
                "SeatId",
                "SeatName"
            );


            shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") ?? new ShoppingCartVM { Tickets = new List<TicketVM>() };

            var existingTicket = shoppingCartVM.Tickets.FirstOrDefault(t => t.FlightId == flightId);

            if (existingTicket != null)
            {
                existingTicket.Count += 1;
            }
            else
            {
                shoppingCartVM.Tickets.Add(new TicketVM
                {
                    FlightId = flight.FlightId,
                    ClassTypes = null,
                    Meals = null,
                    Seasons = null,
                    Seats = null,
                    IssueDate = DateTime.Now,
                    Price = flight.Price,
                    Count = 1
                });
            }

            HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);
            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
