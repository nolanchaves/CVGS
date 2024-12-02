using CVGS.Controllers;
using CVGS.Entities;
using CVGS.Models;
using CVGS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class EventTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private CvgsDbContext _context;
        private EventsController _controller;

        [SetUp]
        public void Setup()
        {
            // Mock the UserManager
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            // Setup the in-memory database for the context
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // Use a fixed name for the in-memory database
                .Options;

            _context = new CvgsDbContext(options);

            // Initialize the controller with the mock context and UserManager
            _controller = new EventsController(_context, _mockUserManager.Object);
        }

        [Test]
        public async Task Test_RegisterForEvent_SuccessfulRegistration()
        {
            // Arrange
            var userId = "user1";
            var random = new Random();

            var eventId = random.Next(1, 1000);
            var testEvent = new Event
            {
                EventId = eventId,
                Name = "Test Event",
                Description = "Test",
                Location = "Test",
                MaxRegistrations = 10,
                Registrations = new List<EventRegistration>()
            };

            var registration = new EventRegistration
            {
                EventId = eventId,
                UserId = userId,
                RegistrationDate = DateTime.UtcNow
            };

            // Add event to the in-memory database
            _context.Events.Add(testEvent);
            await _context.SaveChangesAsync();

            // Setup the mock UserManager to return the userId
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            // Act
            var result = await _controller.Register(eventId);

            // Assert
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual("AllEvents", redirectToActionResult.ActionName);

            // Verify that the registration was added to the database
            var addedRegistration = await _context.EventRegistrations
                .FirstOrDefaultAsync(er => er.EventId == eventId && er.UserId == userId);
            Assert.IsNotNull(addedRegistration);
        }

        [Test]
        public async Task Test_UnregisterForEvent_SuccessfulUnregistration()
        {
            // Arrange
            var userId = "user1";
            var eventId = 1;
            var testEvent = new Event
            {
                EventId = eventId,
                Name = "Test Event",
                Description = "Test",
                Location = "Test",
                MaxRegistrations = 10,
                Registrations = new List<EventRegistration>
                {
                    new EventRegistration
                    {
                        EventId = eventId,
                        UserId = userId,
                        RegistrationDate = DateTime.UtcNow
                    }
                }
            };

            _context.Events.Add(testEvent);
            await _context.SaveChangesAsync();

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(userId);

            // Act
            var result = await _controller.Unregister(eventId);

            // Assert
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual("AllEvents", redirectToActionResult.ActionName);

            var removedRegistration = await _context.EventRegistrations
                .FirstOrDefaultAsync(er => er.EventId == eventId && er.UserId == userId);
            Assert.IsNull(removedRegistration); // Ensure that the registration was removed
        }
    }
}
