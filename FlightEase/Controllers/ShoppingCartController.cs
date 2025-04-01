using Microsoft.AspNetCore.Mvc;
using FlightEase.Services.Interfaces;
using FlightEase.ViewModels;
using FlightEase.Domains.Entities;
using System.Linq;
using System.Threading.Tasks;
using FlightEase.Extentions;

namespace FlightEase.Controllers
{
    public class ShoppingCartController : Controller
    {

        public IActionResult Index()
        {
            ShoppingCartVM? shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartVM == null)
            {
                shoppingCartVM = new ShoppingCartVM();
            }
            return View(shoppingCartVM);
        }
    }
}
