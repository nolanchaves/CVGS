using Microsoft.AspNetCore.Mvc;

namespace CVGS.Controllers
{
    public class FriendsController : Controller
    {
        public IActionResult FriendsList()
        {
            return View("FriendsList");
        }
    }
}
