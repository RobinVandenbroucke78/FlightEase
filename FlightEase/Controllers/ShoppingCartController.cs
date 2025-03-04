using Microsoft.AspNetCore.Mvc;

namespace FlightEase.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
