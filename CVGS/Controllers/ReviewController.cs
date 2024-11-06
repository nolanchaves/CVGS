using Microsoft.AspNetCore.Mvc;

namespace CVGS.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
