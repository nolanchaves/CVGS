using CVGS.Entities;
using CVGS.Models;
using CVGS.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult AllGames()
        {
            var games = _context.Games
                .Select(g => new GameViewModel
                {
                    GameID = g.GameID,
                    Title = g.Title,
                    Platform = g.Platform,
                    Price = g.Price,
                    CoverImageURL = g.CoverImageURL,
                }).ToList();

            return View(games);
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
                    Rating = (float)g.Rating,
                    CoverImageURL = g.CoverImageURL,
                    DownloadSize = g.DownloadSize,
                    Reviews = new List<ReviewDetailViewModel>()
                })
                .FirstOrDefault();


            if (game == null)
            {
                return NotFound();
            }
            else
            {
                var reviews = _reviewService.GetReviewForGame(_context, id, 0, 10);

                foreach (var review in reviews)
                {
                    if (review.Content == null) continue;

                    if (review.UserId == _userManager.GetUserId(User))
                    {
                        game.UserReview = new ReviewDetailViewModel()
                        {
                            DisplayName = _context.Users.Where(u => u.Id == review.UserId).FirstOrDefault().ToString(),
                            Rating = review.Rating,
                            ReviewContent = review.Content
                        };
                    }
                    else
                    {
                        game.Reviews.Add(new ReviewDetailViewModel()
                        {
                            DisplayName = _context.Users.Where(u => u.Id == review.UserId).FirstOrDefault().ToString(),
                            Rating = review.Rating,
                            ReviewContent = review.Content
                        });
                    }
                }
            }

            return View(game);
        }
    }
}
