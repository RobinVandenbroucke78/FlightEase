using Microsoft.AspNetCore.Mvc;
using FlightEase.Services.Interfaces;
using FlightEase.ViewModels;
using FlightEase.Domains.Entities;
using FlightEase.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FlightEase.Util.Mail.Interfaces;
using FlightEase.Util.PDF.Interfaces;

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
        private readonly IEmailSend _emailSend;
        private readonly ICreatePDF _createPDF;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;


        public ShoppingCartController(
            IService<Flight> flightService,
            IService<Ticket> ticketService,
            IService<Seat> seatService,
            IService<ClassType> classTypeService,
            IService<Meal> mealService,
            IService<Season> seasonService,
            IService<Booking> bookingService,
            IEmailSend emailSend,
            ICreatePDF createPDF,
            IWebHostEnvironment hostEnvironment,
            UserManager<IdentityUser> userManager)
        {
            _flightService = flightService;
            _ticketService = ticketService;
            _seatService = seatService;
            _classTypeService = classTypeService;
            _mealService = mealService;
            _seasonService = seasonService;
            _bookingService = bookingService;
            _emailSend = emailSend;
            _createPDF = createPDF;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveTicket(int index, ShoppingCartVM model)
        {
            // Get shopping cart from session
            ShoppingCartVM? shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartVM == null || index < 0 || index >= shoppingCartVM.Tickets.Count)
            {
                return RedirectToAction("Index");
            }

            // Get the ticket at the specified index
            var ticketVM = model.Tickets[0];

            // Request flight for dates and information
            var flight = await _flightService.FindByIdAsync(ticketVM.FlightId);
            if (flight == null)
            {
                ModelState.AddModelError("", "Flight not found.");
                return RedirectToAction("Index");
            }

            // Automatically determine season based on flight departure date
            var departureDateOnly = DateOnly.FromDateTime(flight.DepartureTime);
            var seasons = await _seasonService.GetAllAsync();
            Season? applicableSeason = null;

            // Default to the "Geen" season (ID 3)
            const int DEFAULT_SEASON_ID = 3;
            string defaultSeasonName = "Geen";

            foreach (var season in seasons)
            {
                // Skip seasons with null dates or the default "Geen" season
                if (!season.BeginDate.HasValue || !season.EndDate.HasValue || season.SeasonId == DEFAULT_SEASON_ID)
                    continue;

                if (departureDateOnly >= season.BeginDate.Value && departureDateOnly <= season.EndDate.Value)
                {
                    applicableSeason = season;
                    break;
                }
            }

            // Update ticket price if season is applicable (30% discount)
            double originalPrice = ticketVM.Price;
            if (applicableSeason != null)
            {
                ticketVM.Seasons = applicableSeason.SeasonId;
                ticketVM.SeasonText = applicableSeason.Name;
                // Apply 30% discount
                ticketVM.Price = Math.Round(originalPrice * 0.7, 2); // 30% off
            }
            else
            {
                // Use the default "Geen" season with ID 3
                ticketVM.Seasons = DEFAULT_SEASON_ID;
                ticketVM.SeasonText = defaultSeasonName;
                // No discount applied - keep original price
            }

            // Update the ticket in the shopping cart with form data
            shoppingCartVM.Tickets[index].ClassTypes = ticketVM.ClassTypes;
            shoppingCartVM.Tickets[index].Meals = ticketVM.Meals;
            shoppingCartVM.Tickets[index].Seats = ticketVM.Seats;
            shoppingCartVM.Tickets[index].Seasons = ticketVM.Seasons;
            shoppingCartVM.Tickets[index].SeasonText = ticketVM.SeasonText;
            shoppingCartVM.Tickets[index].Count = ticketVM.Count;
            shoppingCartVM.Tickets[index].FromAirport = ticketVM.FromAirport;
            shoppingCartVM.Tickets[index].ToAirport = ticketVM.ToAirport;
            shoppingCartVM.Tickets[index].Price = ticketVM.Price;
            shoppingCartVM.Tickets[index].IsApproved = true;

            // Load data from dropdown
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
                SeasonId = ticketVM.Seasons.Value, // This will always be a valid ID now
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

            // Season is now set automatically, so we don't need to load it here
            // The SeasonText is already set in the ApproveTicket method
        }

        [HttpGet]
        public IActionResult FinalizeOrder()
        {
            // Get shopping cart from session
            ShoppingCartVM? shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartVM == null || !shoppingCartVM.Tickets.Any(t => t.IsApproved))
            {
                TempData["ErrorMessage"] = "No approved tickets found to finalize order.";
                return RedirectToAction("Index");
            }

            // Pass the cart to the view
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ConfirmOrder()
        {
            try
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

                // Get current user ID and email
                string? userId = _userManager.GetUserId(User);
                string? userEmail = User.Identity?.Name;

                if (string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("User ID is null or empty");
                    TempData["ErrorMessage"] = "User information could not be retrieved. Please try again or log in.";
                    return RedirectToAction("Index");
                }

                List<Booking> bookings = new List<Booking>();

                // Process each ticket
                foreach (var ticketVM in approvedTickets)
                {
                    try
                    {
                        // Find the ticket in database
                        Ticket? ticket = await GetMostRecentTicketAsync(ticketVM);

                        if (ticket != null)
                        {
                            try
                            {
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
                                Console.WriteLine($"Created booking for ticket ID: {ticket.TicketId}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error creating booking: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing ticket: {ex.Message}");
                    }
                }

                // Send email if bookings were created
                if (bookings.Count > 0)
                {
                    try
                    {
                        // Create EmailController instance
                        EmailController emailController = new EmailController(
                            _emailSend,
                            _createPDF,
                            _hostEnvironment,
                            _ticketService,
                            _flightService);

                        // Send a single email with multiple bookings/PDFs
                        await emailController.SendMultipleBookingsEmail(bookings, userEmail);

                        // Clear the shopping cart
                        HttpContext.Session.Remove("ShoppingCart");

                        TempData["SuccessMessage"] = "Your order has been placed successfully!";
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"An error occurred while sending confirmation email: {ex.Message}";
                        return RedirectToAction("Index");
                    }
                }

                TempData["ErrorMessage"] = "No bookings were created. Please check the logs for details.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                return RedirectToAction("Index");
            }
        }

        //Get most recent ticket
        private async Task<Ticket?> GetMostRecentTicketAsync(TicketVM ticketVM)
        {
            try
            {
                var tickets = await _ticketService.GetAllAsync();

                return tickets
                    .Where(t =>
                        t.FlightId == ticketVM.FlightId &&
                        t.SeatId == ticketVM.Seats.Value &&
                        t.MealId == ticketVM.Meals.Value &&
                        t.ClassTypeId == ticketVM.ClassTypes.Value)
                    .OrderByDescending(t => t.TicketId)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMostRecentTicketAsync: {ex.Message}");
                throw; // Re-throw the exception so it's caught by the calling method
            }
        }
    }
}