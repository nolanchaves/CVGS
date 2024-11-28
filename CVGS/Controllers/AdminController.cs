using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CVGS.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;

namespace CVGS.Controllers
{
    [Authorize(Roles = "Admin")] // Only accessible to users in the "Admin" role
    public class AdminController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(CvgsDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult AllReviews()
        {
            var reviews = _context.Review
                                  .Include(r => r.Game)
                                  .Include(r => r.User)
                                  .ToList();
            return View(reviews);
        }

        // Action to view only unapproved reviews
        public IActionResult UnapprovedReviews()
        {
            var unapprovedReviews = _context.Review
                                            .Include(r => r.Game)
                                            .Include(r => r.User)
                                            .Where(r => !r.Approved)
                                            .ToList();
            return View(unapprovedReviews);
        }

        // Action to approve a review
        [HttpPost]
        public async Task<IActionResult> ApproveReview(int id)
        {
            var review = await _context.Review.FindAsync(id);
            if (review != null)
            {
                review.Approved = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(UnapprovedReviews));
        }

        // Action to delete a review
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Review.FindAsync(id);
            if (review != null)
            {
                _context.Review.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AllReviews));
        }

        public IActionResult AddGame()
        {
            var model = new GameViewModel
            {
                PreferenceViewModel = new PreferenceViewModel()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGame(GameViewModel model)
        {
            if (model.PreferenceViewModel == null)
            {
                model.PreferenceViewModel = new PreferenceViewModel();
            }

            if (ModelState.IsValid)
            {
                var game = new Game
                {
                    Title = model.Title,
                    Platform = model.Platform,
                    Price = model.Price,
                    Description = model.Description,
                    Category = model.Category,
                    LanguageSupport = model.LanguageSupport,
                    DownloadSize = model.DownloadSize,
                };

                // Handle the CoverImage file upload
                if (model.CoverImage != null && model.CoverImage.Length > 0) // Use CoverImage here
                {
                    // Define the path to save the image
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    var fileName = Path.GetFileName(model.CoverImage.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CoverImage.CopyToAsync(fileStream);
                    }

                    // Set the CoverImageURL property for the game
                    game.CoverImageURL = "/images/" + fileName; // Store the relative path
                }

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Panel));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteEditGames()
        {
            var games = _context.Games.ToList();
            return View(games);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Game deleted successfully.";
            }

            return RedirectToAction(nameof(DeleteEditGames)); // Redirect to the list action
        }

        [HttpGet]
        public async Task<IActionResult> EditGame(int id)
        {
            var game = await _context.Games
                                      .Where(g => g.GameID == id)
                                      .FirstOrDefaultAsync();

            // If the game is not found, return a NotFound result
            if (game == null)
            {
                return NotFound();
            }

            var model = new GameViewModel
            {
                GameID = game.GameID,
                Title = game.Title,
                Description = game.Description,
                Platform = game.Platform,
                Category = game.Category,
                LanguageSupport = game.LanguageSupport,
                Price = game.Price,
                DownloadSize = game.DownloadSize,
                CoverImageURL = game.CoverImageURL,
                PreferenceViewModel = new PreferenceViewModel()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGame(GameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var game = await _context.Games.FindAsync(model.GameID);
                if (game == null)
                {
                    return NotFound();
                }

                // Update other properties of the game
                game.Title = model.Title;
                game.Description = model.Description;
                game.Platform = model.Platform;
                game.Category = model.Category;
                game.LanguageSupport = model.LanguageSupport;
                game.Price = model.Price;
                game.DownloadSize = model.DownloadSize;

                // If a new cover image is uploaded, process the image and update CoverImageURL
                if (model.CoverImage != null && model.CoverImage.Length > 0)
                {
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    var fileName = Path.GetFileName(model.CoverImage.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);  // Create directory if it doesn't exist
                    }

                    // Save the new cover image file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CoverImage.CopyToAsync(fileStream);  // Save the file to disk
                    }

                    // Update the CoverImageURL property with the new image path
                    game.CoverImageURL = "/images/" + fileName;
                }
                // If no new image is uploaded, the existing CoverImageURL will remain unchanged

                // Update the game entity in the database
                _context.Games.Update(game);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Add a success message to TempData to notify the user
                TempData["SuccessMessage"] = game.Title + " edited successfully.";

                return RedirectToAction(nameof(Panel));
            }

            return View(model);
        }

    }
}
