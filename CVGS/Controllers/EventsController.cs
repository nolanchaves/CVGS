using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;

namespace CVGS.Controllers
{
    public class EventsController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly UserManager<User> _userManager;

        public EventsController(CvgsDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AllEvents()
        {
            var userId = _userManager.GetUserId(User);
            var events = await _context.Events
                .Include(e => e.Registrations)
                .ToListAsync();

            var eventViewModels = events.Select(e => new EventViewModel
            {
                EventId = e.EventId,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                MaxRegistrations = e.MaxRegistrations,
                CurrentRegistrations = e.Registrations.Count,
                IsUserRegistered = e.Registrations.Any(r => r.UserId == userId),
                IsRegistrationOpen = e.Registrations.Count < e.MaxRegistrations && e.Date >= DateOnly.FromDateTime(DateTime.UtcNow)
            });

            return View(eventViewModels);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register(int eventId)
        {
            var userId = _userManager.GetUserId(User);
            var existingRegistration = await _context.EventRegistrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

            var registration = new EventRegistration
            {
                EventId = eventId,
                UserId = userId,
                RegistrationDate = DateTime.UtcNow
            };

            _context.EventRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllEvents");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Unregister(int eventId)
        {
            var userId = _userManager.GetUserId(User);

            var existingRegistration = await _context.EventRegistrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

            if (existingRegistration == null)
            {
                return BadRequest("You are not registered for this event.");
            }

            _context.EventRegistrations.Remove(existingRegistration);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllEvents");
        }
    }
}
