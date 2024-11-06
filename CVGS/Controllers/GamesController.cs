using CVGS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVGS.Controllers
{
    public class GamesController : Controller
    {
        private readonly CvgsDbContext _context;

        public GamesController(CvgsDbContext context)
        {
            _context = context;
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
                    DownloadSize = g.DownloadSize
                })
                .FirstOrDefault();

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }
    }
}
