using Microsoft.AspNetCore.Mvc;

namespace CVGS.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
