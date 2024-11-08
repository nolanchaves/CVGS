using CVGS.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //var user = _userManager.GetUserAsync(User).Result;
            var user = await _userManager.GetUserAsync(User);
            user = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == user.Id);
            var cart = _context.Carts
                               .Include(c => c.CartItems)
                               .ThenInclude(ci => ci.Game) 
                               .FirstOrDefault(c => c.UserID == user.Id);

            if (cart == null)
            {
                return View();
            }

            var userProvince = user.Address?.Province;
            Console.WriteLine("User Province: " + (userProvince ?? "Province is null"));


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

            decimal taxRate = 0.0m;


            if (!string.IsNullOrEmpty(userProvince) && taxRates.ContainsKey(userProvince))
            {
                taxRate = taxRates[userProvince];
            }

            decimal totalBeforeTax = cart.CartItems.Sum(item => item.Quantity * item.Game.Price);

            decimal taxAmount = totalBeforeTax * taxRate;
            decimal totalAmount = totalBeforeTax + taxAmount;
            decimal totalAfterTax = totalBeforeTax + taxAmount;

            ViewBag.TotalBeforeTax = totalBeforeTax;
            ViewBag.TaxAmount = taxAmount;
            ViewBag.TotalAfterTax = totalAfterTax;
            ViewBag.TaxRate = taxRate;

            return View(cart);
        }



        [HttpPost]
        public async Task<IActionResult> AddToCart(int gameId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to add items to your cart.";
                return RedirectToAction("Login", "Account");
            }

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefault(c => c.UserID == user.Id);

            if (cart == null)
            {
                cart = new Cart { UserID = user.Id, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.GameId == gameId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                TempData["SuccessMessage"] = $"{cartItem.Game.Title} is now in your cart with a quantity of {cartItem.Quantity}.";
            }
            else
            {
                cart.CartItems.Add(new CartItem { GameId = gameId, Quantity = 1 });
                var game = _context.Games.FirstOrDefault(g => g.GameID == gameId);
                TempData["SuccessMessage"] = $"{game.Title} has been added to your cart.";
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

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserID == user.Id);

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

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserID == user.Id);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ViewCart", "Cart");
        }

        public async Task<int> GetCartItemCount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return 0;
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == user.Id);

            return cart?.CartItems.Sum(item => item.Quantity) ?? 0;
        }
    }
}
