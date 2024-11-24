using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CVGS.Models; // Adjust based on your namespace
using System.Linq;
using CVGS;
using CVGS.Entities;

namespace CVGS.Controllers
{
    public class OrderController : Controller
    {
        private readonly CvgsDbContext _context;

        public OrderController(CvgsDbContext context)
        {
            _context = context;
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            // Fetch the order by ID
            var order = _context.Orders
                                .Include(o => o.OrderDetails)
                                .ThenInclude(od => od.Game) // Assuming Game info is needed
                                .Include(o => o.ShippingAddress) // Include user shipping address
                                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found."); // Handle invalid order ID
            }

            // Prepare the view model
            var viewModel = new OrderViewModel
            {
                OrderedDetails = order.OrderDetails.ToList(),
                Subtotal = order.OrderDetails.Sum(od => od.Price * od.Quantity),
                TaxRate = order.TotalPrice > 0 ? (order.TotalPrice - order.OrderDetails.Sum(od => od.Price * od.Quantity)) / order.OrderDetails.Sum(od => od.Price * od.Quantity) : 0, // Calculate from totals
                TaxAmount = order.TotalPrice - order.OrderDetails.Sum(od => od.Price * od.Quantity),
                TotalPrice = order.TotalPrice
            };

            ViewBag.UserDetails = new
            {
                Name = $"{order.ShippingAddress.User.FullName}",
                Address = $"{order.ShippingAddress.ShippingStreetAddress}, {order.ShippingAddress.ShippingCity}, {order.ShippingAddress.ShippingProvince}, {order.ShippingAddress.ShippingPostalCode}",
                Phone = order.ShippingAddress.ShippingPhoneNumber
            };

            return View(viewModel);
        }

    }
}