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
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.GetUserAsync(User);
            user = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == userId);
            var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Game).FirstOrDefaultAsync(c => c.UserID == userId);

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Reload the cart items to include in the model
                model.CartItems = await _context.CartItems
                    .Include(c => c.Game)
                    .Where(c => c.Cart.UserID == userId)
                    .ToListAsync();

                // Optionally recalculate totals if needed
                decimal recalculatedTaxRate = GetUserTaxRate(user);
                decimal totalBeforeTax = model.CartItems.Sum(item => item.Quantity * item.Game.Price);
                model.TotalBeforeTax = totalBeforeTax; // Set total before tax if you want to show it
                model.TotalPrice = totalBeforeTax + (totalBeforeTax * GetUserTaxRate(await _userManager.GetUserAsync(User))); // Example tax calculation
                decimal taxAmount = totalBeforeTax * recalculatedTaxRate;
                decimal totalAfterTax = totalBeforeTax + taxAmount;

                model.TotalBeforeTax = totalBeforeTax;
                model.TaxAmount = taxAmount;
                model.TaxRate = recalculatedTaxRate;
                model.TotalPrice = totalAfterTax;

                return View("Checkout", model); // Return to checkout with the updated model
            }

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("ViewCart");
            }

            string ccType = GetCreditCardType(model.CreditCardNumber);

            // Create a new order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = model.TotalPrice,
                TaxAmount = model.TaxAmount,
                PaymentMethod = ccType,
                OrderDetails = cart.CartItems.Select(item => new OrderDetail
                {
                    GameId = item.GameId,
                    Quantity = item.Quantity,
                    Price = item.Game.Price,
                    GameType = item.GameType
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems); // Clear cart items after the order is placed
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your order has been placed successfully!";
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).ThenInclude(oi => oi.Game).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
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

        public static string GetCreditCardType(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || !long.TryParse(cardNumber.Replace(" ", "").Replace("-", ""), out _))
            {
                return "Invalid card number";
            }

            // Remove spaces or dashes if present
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            // Check for length and prefix to determine card type
            if (cardNumber.Length == 16)
            {
                if (cardNumber.StartsWith("4"))
                    return "Visa";
                else if (cardNumber.StartsWith("51") || cardNumber.StartsWith("52") ||
                         cardNumber.StartsWith("53") || cardNumber.StartsWith("54") ||
                         cardNumber.StartsWith("55"))
                    return "MasterCard";
                else if (cardNumber.StartsWith("6011") || cardNumber.StartsWith("65") ||
                         (cardNumber.StartsWith("622") && (int.Parse(cardNumber.Substring(2, 1)) >= 1 && int.Parse(cardNumber.Substring(2, 1)) <= 9)))
                    return "Discover";
            }
            else if (cardNumber.Length == 15)
            {
                if (cardNumber.StartsWith("34") || cardNumber.StartsWith("37"))
                    return "American Express";
            }
            else if (cardNumber.Length == 13)
            {
                if (cardNumber.StartsWith("4"))
                    return "Visa";
            }

            return "Unknown card type";
        }


    }
}
