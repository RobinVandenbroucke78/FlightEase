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
using System.Net.Sockets;
using Microsoft.AspNetCore.Authorization;
using FlightEase.Util.Mail.Interfaces;
using FlightEase.Util.PDF.Interfaces;
using System.Security.Claims;

namespace FlightEase.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IService<Flight> _flightService;
        private readonly IService<Ticket> _ticketService;
        private readonly IService<Seat> _seatService;
        private readonly IService<ClassType> _classTypeService;
        private readonly IService<Meal> _mealService;
        private readonly IService<Season> _seasonService;
        private readonly IService<Booking> _bookingService;
        private readonly IEmailSend _emailService;
        private readonly ICreatePDF _pdfService;
        private readonly IMapper _mapper;

        public ShoppingCartController(
            IService<Flight> flightService,
            IService<Ticket> ticketService,
            IService<Seat> seatService,
            IService<ClassType> classTypeService,
            IService<Meal> mealService,
            IService<Season> seasonService,
            IService<Booking> bookingService,
            IEmailSend emailService,
            ICreatePDF pdfService,
            IMapper mapper)
        {
            _flightService = flightService;
            _ticketService = ticketService;
            _seatService = seatService;
            _classTypeService = classTypeService;
            _mealService = mealService;
            _seasonService = seasonService;
            _bookingService = bookingService;
            _emailService = emailService;
            _pdfService = pdfService;
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
            shoppingCartVM.Tickets[index] = ticketVM;
            shoppingCartVM.Tickets[index].IsApproved = true;

            //load data from dropdown
            await LoadSelectedItems(shoppingCartVM.Tickets[index]);

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

        private async Task LoadSelectedItems(TicketVM ticket)
        {
            // Load text values for the selected dropdown items
            if (ticket.ClassTypes.HasValue)
            {
                var classType = await _classTypeService.FindByIdAsync(ticket.ClassTypes.Value);
                ticket.ClassTypeText = classType?.ClassName;
            }

            if (ticket.Meals.HasValue)
            {
                var meal = await _mealService.FindByIdAsync(ticket.Meals.Value);
                ticket.MealText = meal?.MealName;
            }

            if (ticket.Seats.HasValue)
            {
                var seat = await _seatService.FindByIdAsync(ticket.Seats.Value);
                ticket.SeatText = seat?.SeatNumber;
            }

            if (ticket.Seasons.HasValue)
            {
                var season = await _seasonService.FindByIdAsync(ticket.Seasons.Value);
                ticket.SeasonText = season?.Name;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // Ensure user is logged in
        public async Task<IActionResult> FinalizeOrder()
        {
            // Get shopping cart from session
            ShoppingCartVM? shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartVM == null || !shoppingCartVM.Tickets.Any(t => t.IsApproved))
            {
                TempData["ErrorMessage"] = "No approved tickets found to finalize order.";
                return RedirectToAction("Index");
            }

            // Get approved tickets
            var approvedTickets = shoppingCartVM.Tickets.Where(t => t.IsApproved).ToList();

            // Get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.Identity.Name;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "User information not found. Please login again.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            List<Booking> bookings = new List<Booking>();

            // Create bookings for each approved ticket
            foreach (var ticketVM in approvedTickets)
            {
                // Find the ticket in database
                var ticket = await _ticketService.FindByIdAsync(ticketVM.TicketId);
                if (ticket == null) continue;

                // Create booking
                var booking = new Booking
                {
                    UserId = userId,
                    TicketId = ticket.TicketId,
                    BookingDate = DateTime.Now,
                    BookingName = $"Booking for flight {ticket.FlightId} - {ticketVM.FromAirport} to {ticketVM.ToAirport}",
                    Price = ticket.Price
                };

                // Add to database
                await _bookingService.AddAsync(booking);
                bookings.Add(booking);
            }

            if (bookings.Count > 0)
            {
                try
                {
                    // Send email
                    var emailService = HttpContext.RequestServices.GetRequiredService<IEmailSend>();
                    string subject = "Your FlightEase Booking Confirmation";
                    string message = $@"
                <h2>Thank you for your booking with FlightEase!</h2>
                <p>Your booking has been confirmed. Please find attached your booking receipt.</p>
                <p>Details:</p>
                <ul>
                    {string.Join("", bookings.Select(b => $"<li>Booking #{b.BookingId}: {b.BookingName} - ${b.Price}</li>"))}
                </ul>
                <p>Total: ${bookings.Sum(b => b.Price)}</p>
                <p>Thank you for choosing FlightEase for your travel needs!</p>
            ";

                    await emailService.SendEmailAsync(
                        userEmail,
                        subject,
                        message
                    );

                    // Remove approved tickets from cart or clear cart
                    shoppingCartVM.Tickets.RemoveAll(t => t.IsApproved);
                    if (shoppingCartVM.Tickets.Count == 0)
                    {
                        HttpContext.Session.Remove("ShoppingCart");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);
                    }

                    TempData["SuccessMessage"] = "Your order has been successfully finalized and a confirmation email has been sent.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while processing your order: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }

            TempData["ErrorMessage"] = "No bookings were created. Please try again.";
            return RedirectToAction("Index");
        }
    }
}