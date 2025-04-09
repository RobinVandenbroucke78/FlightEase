using Microsoft.AspNetCore.Mvc;
using FlightEase.Services.Interfaces;
using FlightEase.ViewModels;
using FlightEase.Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using FlightEase.Services.Interfaces;
using FlightEase.ViewModels;
using FlightEase.Domains.Entities;
using System.Linq;
using System.Threading.Tasks;
using FlightEase.Extentions;
using AutoMapper;
using FlightEase.Services;

namespace FlightEase.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IService<Flight> _flightService;
        private readonly IService<Ticket> _ticketService;
        private readonly IService<Seat> _seatService;
        private readonly IMapper _mapper;

        public ShoppingCartController(
            IService<Flight> flightService,
            IService<Ticket> ticketService,
            IService<Seat> seatService,
            IMapper mapper)
        {
            _flightService = flightService;
            _ticketService = ticketService;
            _seatService = seatService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ShoppingCartVM? shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartVM == null)
            {
                shoppingCartVM = new ShoppingCartVM();
                return View(shoppingCartVM);
            }

            // Load flight details for each ticket if needed
            foreach (var ticket in shoppingCartVM.Tickets)
            {
                if (string.IsNullOrEmpty(ticket.FromAirport))
                {
                    var flight = await _flightService.FindByIdAsync(ticket.FlightId);
                    if (flight != null)
                    {
                        ticket.FromAirport = flight.FromAirport.City.CityName;
                        ticket.ToAirport = flight.ToAirport.City.CityName;
                    }
                }
            }

            return View(shoppingCartVM);
        }

        [HttpGet]
        public IActionResult RemoveTicket(int index)
        {
            // Get shopping cart from session
            ShoppingCartVM? shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartVM == null || index < 0 || index >= shoppingCartVM.Tickets.Count)
            {
                return RedirectToAction("Index");
            }

            // Remove the ticket at the specified index
            shoppingCartVM.Tickets.RemoveAt(index);

            // Save updated cart to session
            HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ApproveTicket(int index, ShoppingCartVM model)
        {
            // Get shopping cart from session
            ShoppingCartVM? shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartVM == null || index < 0 || index >= shoppingCartVM.Tickets.Count)
            {
                return RedirectToAction("Index");
            }

            // Get the ticket at the specified index
            var ticketVM = model.Tickets[index];

            // Get the selected seat to retrieve the seat number
            var seat = await _seatService.FindByIdAsync(ticketVM.Seats ?? 0);
            if (seat == null)
            {
                ModelState.AddModelError("", "Please select a valid seat.");
                return RedirectToAction("Index");
            }

            // Create a real ticket in the database
            Ticket newTicket = new Ticket
            {
                FlightId = ticketVM.FlightId,
                Price = ticketVM.Price,
                SeatId = ticketVM.Seats ?? 0,
                SeatNumber = seat.SeatNumber,
                MealId = ticketVM.Meals ?? 0,
                ClassTypeId = ticketVM.ClassTypes ?? 0,
                SeasonId = ticketVM.Seasons ?? 0,
                IssueDate = DateOnly.FromDateTime(DateTime.Now)
            };

            // Create multiple tickets based on Count if needed
            for (int i = 0; i < ticketVM.Count; i++)
            {
                await _ticketService.AddAsync(newTicket);
            }

            // Save updated cart to session
            HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);

            TempData["SuccessMessage"] = "Ticket has been approved and created successfully!";
            return RedirectToAction("Index");
        }
    }
}