using CVGS.Entities;
using CVGS.Models;
using CVGS.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CVGS.Controllers
{
    public class GamesController : Controller
    {
        private readonly CvgsDbContext _context;
        readonly UserManager<User> _userManager;
        readonly ReviewService _reviewService;

        public GamesController(CvgsDbContext context, ReviewService revService, UserManager<User> userManager)
        {
            _context = context;
            _reviewService = revService;
            _userManager = userManager;
        }

        public IActionResult AllGames(string searchQuery)
        {
            var gamesQuery = _context.Games.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                gamesQuery = gamesQuery.Where(g => g.Title.Contains(searchQuery));
            }

            var games = gamesQuery
                .Select(g => new GameViewModel
                {
                    GameID = g.GameID,
                    Title = g.Title,
                    Platform = g.Platform,
                    Price = g.Price,
                    CoverImageURL = g.CoverImageURL
                })
                .ToList();

            ViewBag.SearchQuery = searchQuery;

            return View(games);
        }
        
        public IActionResult RecommendedGames()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("AllGames");
            var _userId = _userManager.GetUserId(User);

            var userPref = _context.Users.Include(u=>u.Preferences).FirstOrDefault(u=>u.Id==_userId).Preferences;

            var games = _context.Games
                .Where(g=>
                    userPref.LanguagePreferences.Any(p=>g.LanguageSupport.Contains(p))||
                    userPref.FavouritePlatforms.Any(p=>g.Platform.Contains(p))||
                    userPref.FavouriteGameCategories.Any(p=>g.Category.Contains(p))
                )
                .Select(g => new GameViewModel
                {
                    GameID = g.GameID,
                    Title = g.Title,
                    Platform = g.Platform,
                    Price = g.Price,
                    CoverImageURL = g.CoverImageURL
                })
                .ToList();


            return View("AllGames",games);
        }

        public IActionResult GameDetails(int id)
        {
            var game = _context.Games
                .Where(g => g.GameID == id)
                .Select(g => new GameViewModel
                {
                    GameID = g.GameID,
                    Title = g.Title,
                    Description = g.Description,
                    Platform = g.Platform,
                    Category = g.Category,
                    LanguageSupport = g.LanguageSupport,
                    Price = g.Price,
                    Rating = (float)(g.Review.Any() ? g.Review.Average(r => r.Rating) : 0),
                    CoverImageURL = g.CoverImageURL,
                    DownloadSize = g.DownloadSize,
                    Reviews = new List<ReviewDetailViewModel>()
                })
                .FirstOrDefault();

            if (game == null)
            {
                return NotFound();
            }

            var reviews = _reviewService.GetReviewForGame(_context, id, 0, 10)
                .Where(r => r.Approved) 
                .ToList();

            foreach (var review in reviews)
            {
                if (string.IsNullOrEmpty(review.Content)) continue;

                var user = _context.Users.FirstOrDefault(u => u.Id == review.UserId);
                if (user == null) continue;

                var reviewDetail = new ReviewDetailViewModel()
                {
                    DisplayName = user.UserName, 
                    Rating = review.Rating,
                    ReviewContent = review.Content
                };

                if (review.UserId == _userManager.GetUserId(User))
                {
                    game.UserReview = reviewDetail;
                }
                else
                {
                    game.Reviews.Add(reviewDetail);
                }
            }

            return View(game);
        }
    }
}
