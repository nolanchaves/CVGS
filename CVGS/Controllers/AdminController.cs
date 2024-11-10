using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CVGS.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CVGS.Controllers
{
    [Authorize(Roles = "Admin")] // Only accessible to users in the "Admin" role
    public class AdminController : Controller
    {
        private readonly CvgsDbContext _context;

        public AdminController(CvgsDbContext context)
        {
            _context = context;
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
    }
}
