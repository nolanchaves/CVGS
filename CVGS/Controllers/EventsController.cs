using Microsoft.AspNetCore.Mvc;

namespace CVGS.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult EventsList()
        {
            return View("EventsList");
        }
    }
}
