using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CVGS.Controllers
{
    public class CartController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(CvgsDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ViewCart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == user.Id);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync(c => c.UserID == user.Id);

            if (cart == null)
            {
                return View();
            }

            decimal taxRate = GetUserTaxRate(user);
            decimal totalBeforeTax = cart.CartItems.Sum(item => item.Quantity * item.Game.Price);
            decimal taxAmount = totalBeforeTax * taxRate;
            decimal totalAfterTax = totalBeforeTax + taxAmount;

            ViewBag.TotalBeforeTax = totalBeforeTax;
            ViewBag.TaxAmount = taxAmount;
            ViewBag.TotalAfterTax = totalAfterTax;
            ViewBag.TaxRate = taxRate;

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int gameId, string gameType)
        {
            if (string.IsNullOrEmpty(gameType))
            {
                TempData["ErrorMessage"] = "Please select a game type before adding to the cart.";
                return RedirectToAction("GameDetails", "Games", new { id = gameId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to add items to your cart.";
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync(c => c.UserID == user.Id);

            if (cart == null)
            {
                cart = new Cart { UserID = user.Id, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.GameId == gameId && ci.GameType == gameType);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                TempData["SuccessMessage"] = $"{cartItem.Game.Title} ({gameType}) is now in your cart with a quantity of {cartItem.Quantity}.";
            }
            else
            {
                var game = await _context.Games.FindAsync(gameId);
                if (game == null)
                {
                    TempData["ErrorMessage"] = "Game not found.";
                    return RedirectToAction("AllGames", "Games");
                }
                cart.CartItems.Add(new CartItem { GameId = gameId, GameType = gameType, Quantity = 1 });
                TempData["SuccessMessage"] = $"{game.Title} ({gameType}) has been added to your cart.";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AllGames", "Games");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == user.Id);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.GameId == gameId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("ViewCart", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == user.Id);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewCart", "Cart");
        }

        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Include(u => u.Address).FirstOrDefault(u => u.Id == userId);

            var cartItems = _context.CartItems.Include(c => c.Game).Where(c => c.Cart.UserID == userId).ToList();

            decimal taxRate = GetUserTaxRate(user);

            var totalBeforeTax = cartItems.Sum(item => item.Game.Price * item.Quantity);
            var taxAmount = totalBeforeTax * taxRate;
            var totalAfterTax = totalBeforeTax + taxAmount;

            var viewModel = new CheckoutViewModel
            {
                UserId = userId,
                CartItems = cartItems,
                TotalPrice = totalAfterTax,
                TaxAmount = taxAmount,
                TaxRate = taxRate,
                TotalBeforeTax = totalBeforeTax
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCheckout(CheckoutViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.GetUserAsync(User);
            user = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == userId);

            var shippingAddress = await _context.ShippingAddresses.FirstOrDefaultAsync(a => a.ShippingAddressId == model.ShippingAddressId);

            if (shippingAddress == null)
            {
                ModelState.AddModelError("", "Invalid shipping address.");
                return View(model);
            }

            model.CartItems = await _context.CartItems
                .Include(c => c.Game)
                .Where(c => c.Cart.UserID == userId)
                .ToListAsync();

            decimal recalculatedTaxRate = GetUserTaxRate(user);

            if (!ModelState.IsValid)
            {
                decimal totalBeforeTax = model.CartItems.Sum(item => item.Quantity * item.Game.Price);
                decimal taxAmount = totalBeforeTax * recalculatedTaxRate;
                decimal totalAfterTax = totalBeforeTax + taxAmount;

                model.TotalBeforeTax = totalBeforeTax;
                model.TaxAmount = taxAmount;
                model.TaxRate = recalculatedTaxRate;
                model.TotalPrice = totalAfterTax;

                return View(model);
            }

            var order = new Order
            {
                UserId = model.UserId,
                OrderDate = DateTime.Now,
                TotalPrice = model.TotalPrice,
                PaymentMethod = "Credit Card",
                ShippingAddressId = model.ShippingAddressId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var cartItem in model.CartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    GameId = cartItem.GameId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Game.Price,
                    GameType = cartItem.GameType,
                };
                _context.OrderDetails.Add(orderDetail);
            }

            await _context.SaveChangesAsync();

            var userCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserID == model.UserId);
            if (userCart != null)
            {
                _context.CartItems.RemoveRange(userCart.CartItems);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }


        private decimal GetUserTaxRate(User user)
        {
            if (user == null || user.Address == null || string.IsNullOrEmpty(user.Address.Province))
            {
                return 0.0m; // Default tax rate if user or province is not set
            }

            var taxRates = new Dictionary<string, decimal>
            {
                { "Alberta", 0.05m },
                { "British Columbia", 0.12m },
                { "Manitoba", 0.13m },
                { "New Brunswick", 0.15m },
                { "Newfoundland and Labrador", 0.15m },
                { "Nova Scotia", 0.15m },
                { "Ontario", 0.13m },
                { "Prince Edward Island", 0.15m },
                { "Quebec", 0.14975m },
                { "Saskatchewan", 0.11m },
                { "Northwest Territories", 0.05m },
                { "Nunavut", 0.05m },
                { "Yukon", 0.05m },
                { "Québec", 0.14975m }
            };

            var province = user.Address.Province;
            return taxRates.TryGetValue(province, out var rate) ? rate : 0.0m;
        }

    }
}
