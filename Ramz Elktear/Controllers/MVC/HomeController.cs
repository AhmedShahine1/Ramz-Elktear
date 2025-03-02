using Microsoft.AspNetCore.Mvc;

namespace Ramz_Elktear.Controllers.MVC
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
