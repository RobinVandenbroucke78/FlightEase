using Microsoft.AspNetCore.Mvc;

namespace FlightEase.Controllers
{
    public class FlightController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
