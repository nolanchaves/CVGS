using CVGS.Entities;
using CVGS.Models;
using CVGS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CVGS.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly CvgsDbContext _context;

        public OrdersController(CvgsDbContext context)
        {
            _context = context;
        }

        public IActionResult OrderList()
        {
            // Use the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch the user's orders
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new UserOrdersViewModel
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    PaymentMethod = o.PaymentMethod
                })
                .ToList();

            return View(orders);
        }

        public IActionResult OrderDetails(int orderId)
        {
            // Fetch the order details by orderId
            var orderDetails = _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Select(od => new OrderDetailViewModel
                {
                    GameTitle = od.Game.Title,
                    GameImageUrl = od.Game.CoverImageURL,
                    Price = od.Price,
                    Quantity = od.Quantity,
                    GameType = od.GameType
                })
                .ToList();

            if (!orderDetails.Any())
            {
                return NotFound();
            }

            return View(orderDetails);
        }
    }
}
