using FlightEase.Services.Interfaces;
using FlightEase.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FlightEase.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CityVM cityVM)
        {
            try
            {
                var lstHotels = await _hotelService.GetHotelsByCityAsync(cityVM.City);
                if (lstHotels != null)
                {
                    List<HotelVM> hotelVMs = new List<HotelVM>();
                    foreach (var hotel in lstHotels)
                    {
                        var hotelVM = new HotelVM();
                        hotelVM.Name = hotel.Name;
                        hotelVM.AddressVM.CountryCode = hotel?.Address?.CountryCode;
                        hotelVM.GeoCodeVM.Longitude = hotel?.GeoCode?.Longitude;
                        hotelVM.GeoCodeVM.Latitude = hotel?.GeoCode?.Latitude;
                        hotelVM.LastUpdate = hotel?.LastUpdate;
                        hotelVMs.Add(hotelVM);
                    }
                    return View("Hotels", hotelVMs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage =
                    "Er is een fout opgetreden bij het ophalen van de hotels. Probeer het later opnieuw.";
                return View("Error");
            }
            return View();
        }
    }
}