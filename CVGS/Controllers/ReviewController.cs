using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace CVGS.Controllers
{
    public class ReviewController : Controller
    {
        CvgsDbContext _context;
        UserManager<User> _userManager;
        public ReviewController(CvgsDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AddReview(int id)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            if(_context.Review.Any(r => r.UserId == _userManager.GetUserId(User) && r.GameId == id))
                return RedirectToAction("GameDetails", "Games", new { id = id });

            ReviewRateViewModel rvm = new ReviewRateViewModel() { GameId = id};
            return View("AddReview",rvm);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewRateViewModel model)
        {
            Debug.WriteLine("review: added");
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(User);
                    var exists =await _context.Review.FirstOrDefaultAsync<Review>(r=>r.UserId==userId && r.GameId== model.GameId);

                    if (exists == null)
                    {
                        exists = new Review();
                        await _context.Review.AddAsync(exists);
                        //_context.Entry(model).State = EntityState.Added;
                        exists.UserId = userId;
                        exists.GameId = model.GameId;
                        exists.Rating = model.Rate;
                        exists.Content = model.Review;

                        await _context.SaveChangesAsync();
                        Debug.WriteLine("Review: added");
                    }
                    else
                    {
                        return RedirectToAction("GameDetails", "Games", new { id = model.GameId });
                    }

                }
                catch (Exception ex)
                {
                    // Log the exception
                    Debug.WriteLine(ex.Message);
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }

            return RedirectToAction("GameDetails", "Games", new { id = model.GameId });
        }

    }
}
