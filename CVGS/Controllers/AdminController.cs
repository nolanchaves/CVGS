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
using Microsoft.AspNetCore.Identity;
using CVGS.Service;
using System.Diagnostics;

namespace CVGS.Controllers
{
    [Authorize(Roles = "Admin")] // Only accessible to users in the "Admin" role
    public class AdminController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(CvgsDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;

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

        public async Task<IActionResult> AllEvents()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        public IActionResult AddEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(EventViewModel eventViewModel)
        {
            // Check if a similar event already exists in the database
            var existingEvent = await _context.Events
                .Where(e => e.Name == eventViewModel.Name && e.Date == eventViewModel.Date)
                .FirstOrDefaultAsync();

            // If the event already exists, add a model state error
            if (existingEvent != null)
            {
                ModelState.AddModelError(string.Empty, "An event with the same name and date already exists.");
                return View(eventViewModel);
            }

            if (ModelState.IsValid)
            {
                var eventItem = new Event
                {
                    Name = eventViewModel.Name,
                    Date = eventViewModel.Date,
                    Location = eventViewModel.Location,
                    Description = eventViewModel.Description,
                    MaxRegistrations = eventViewModel.MaxRegistrations
                };

                _context.Add(eventItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Panel));
            }

            return View(eventViewModel);
        }

        public async Task<IActionResult> EditEvent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(int id, Event eventItem)
        {
            if (id != eventItem.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(eventItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllEvents));
            }
            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem != null)
            {
                _context.Events.Remove(eventItem); 
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AllEvents));
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
                    Reviews = new List<ReviewDetailViewModel>(),
                    UserReview = null
                })
                .FirstOrDefault();

            return View(game);
        }

        public async Task<IActionResult> AllUsers()
        {
            // Get all users
            var users = await _userManager.Users.ToListAsync();

            // Create a list to store users with their roles
            var userRoles = new List<UserRoleViewModel>();

            // For each user, get their roles and store them
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userRoleViewModel = new UserRoleViewModel
                {
                    User = user,
                    Roles = roles
                };
                userRoles.Add(userRoleViewModel);
            }

            return View(userRoles);
        }


        public async Task<IActionResult> UserDetails(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                return BadRequest("Display Name is required.");
            }

            var user = await _userManager.Users
                .Include(u => u.Address)
                .Include(u => u.ShippingAddress)
                .Include(u => u.Preferences)
                .FirstOrDefaultAsync(u => u.UserName == displayName);

            if (user == null)
            {
                Debug.WriteLine("User not found");
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                ActualName = user.FullName ?? "N/A",
                Gender = user.Gender ?? "N/A",
                BirthDate = user.BirthDate.HasValue
                    ? (DateOnly)user.BirthDate.Value
                    : DateOnly.MinValue,

                ReceivePromotionalEmails = user.ReceivePromotionalEmails ?? false,

                Preferences = new PreferenceViewModel
                {
                    FavouritePlatforms = user.Preferences?.FavouritePlatforms ?? new List<string>(),
                    FavouriteGameCategories = user.Preferences?.FavouriteGameCategories ?? new List<string>(),
                    LanguagePreferences = user.Preferences?.LanguagePreferences ?? new List<string>()
                },

                Address = new AddressViewModel
                {
                    PhoneNumber = user.Address?.PhoneNumber ?? "N/A",
                    StreetAddress = user.Address?.StreetAddress ?? "N/A",
                    AptSuite = user.Address?.AptSuite ?? "N/A",
                    City = user.Address?.City ?? "N/A",
                    Province = user.Address?.Province ?? "N/A",
                    PostalCode = user.Address?.PostalCode ?? "N/A",
                    Country = user.Address?.Country ?? "N/A",
                    DeliveryInstructions = user.Address?.DeliveryInstructions ?? "N/A",
                    SameAsShippingAddress = user.Address?.SameAsShippingAddress ?? false,
                    ShippingPhoneNumber = user.ShippingAddress?.ShippingPhoneNumber ?? "N/A",
                    ShippingStreetAddress = user.ShippingAddress?.ShippingStreetAddress ?? "N/A",
                    ShippingAptSuite = user.ShippingAddress?.ShippingAptSuite ?? "N/A",
                    ShippingCity = user.ShippingAddress?.ShippingCity ?? "N/A",
                    ShippingProvince = user.ShippingAddress?.ShippingProvince ?? "N/A",
                    ShippingPostalCode = user.ShippingAddress?.ShippingPostalCode ?? "N/A",
                    ShippingCountry = user.ShippingAddress?.ShippingCountry ?? "N/A"
                },

                FavouritePlatforms = user.Preferences?.FavouritePlatforms ?? new List<string>(),
                FavouriteGameCategories = user.Preferences?.FavouriteGameCategories ?? new List<string>(),
                LanguagePreferences = user.Preferences?.LanguagePreferences ?? new List<string>()
            };
            return View(model);
        }


        public async Task<IActionResult> DeleteUser(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == displayName);
            if (user == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (address != null)
            {
                address.UserId = null;
                _context.Addresses.Update(address);
                await _context.SaveChangesAsync();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
                return RedirectToAction("Panel");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete the user.";
                return RedirectToAction("Panel");
            }
        }

        public async Task<IActionResult> AllUsersWithWishlist()
        {
            var usersWithWishlist = await _context.Users
                .Include(u => u.Wishlist)
                .ToListAsync();

            var viewModel = usersWithWishlist.Select(u => new WishlistViewModel
            {
                GameIdList = u.Wishlist.Select(w => w.GameId).ToList()
            }).ToList();

            return View(viewModel);
        }
    }
}
