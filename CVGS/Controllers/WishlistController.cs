using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CVGS.Controllers
{
    public class WishlistController : Controller
    {

        CvgsDbContext _context;
        UserManager<User> _userManager;
        CartController _cartController;

        public WishlistController(CvgsDbContext context, UserManager<User> userManager, CartController cartController)
        {
            _context = context;
            _userManager = userManager;
            _cartController = cartController;
        }
        public IActionResult Index()
        {
            return RedirectToAction("ViewAllWishlist");
        }

        [HttpGet]
        public IActionResult ViewAllWishlist()
        {
            try
            {
                if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
                var _userId = _userManager.GetUserId(User);

                List<Wishlist> wishlist = _context.Wishlist.Where(r => r.UserId == _userId).ToList();
                List<GameViewModel> games = new List<GameViewModel>();

                foreach (Wishlist w in wishlist)
                {
                    var game = _context.Games
                        .Select(g => new GameViewModel()
                        {
                            GameID = g.GameID,
                            Title = g.Title,
                            Platform = g.Platform,
                            Price = g.Price,
                            CoverImageURL = g.CoverImageURL,
                        })
                        .Where(g=>g.GameID == w.GameId)
                        .FirstOrDefault();   
                    
                    if(game!=null)games.Add(game);
                }

                return View(games);
            }
            catch (Exception ex) {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public IActionResult RemoveWishlist(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
                var _userId = _userManager.GetUserId(User);

                Wishlist wishlist = _context.Wishlist.Where(r => r.UserId == _userId && r.GameId == id).FirstOrDefault();

                if (wishlist == null)
                {
                    return RedirectToAction("ViewAllWishlist");
                }

                _context.Wishlist.Remove(wishlist);

                _context.SaveChanges();

                return RedirectToAction("ViewAllWishlist");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public async Task<IActionResult> AddWishlist(int id)
        {
            try
            {
                //Debug.WriteLine(User.Identity.IsAuthenticated);
                if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
                var _userId = _userManager.GetUserId(User);

                var exists = await _context.Wishlist.FirstOrDefaultAsync(r => r.UserId == _userId && r.GameId == id);

                if (exists == null)
                {
                    exists = new Wishlist();
                    await _context.Wishlist.AddAsync(exists);
                    //_context.Entry(model).State = EntityState.Added;
                    exists.UserId = _userId;
                    exists.GameId = id;

                    await _context.SaveChangesAsync();
                    Debug.WriteLine("Wishlist: added");
                }
                else
                {
                    Debug.WriteLine("Wishlist exists");
                }
                var game = _context.Games.FirstOrDefault(g => g.GameID == exists.GameId);
                TempData["SuccessMessage"] = $"{game.Title} has been added to your Wishlist.";
                return RedirectToAction("AllGames", "Games");

            }
            catch (Exception ex)
            {
                // Log the exception
                Debug.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
