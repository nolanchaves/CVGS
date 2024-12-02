using CVGS.Controllers;
using CVGS.Entities;
using CVGS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;
using Microsoft.AspNetCore.Mvc;


namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class AdminTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<SignInManager<User>> _mockSignInManager;
        private Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private Mock<IWebHostEnvironment> _mockHostEnv;
        private CvgsDbContext _context;
        private AdminController _adminController;

        [SetUp]
        public void Setup()
        {
            // Mock the UserManager
            var userStoreMock = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Mock the SignInManager
            _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, null, null, null, null, null, null);

            // Mock IWebHostEnvironment
            _mockHostEnv = new Mock<IWebHostEnvironment>();

            // In-memory database setup
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: "CvgsTestDb")
                .Options;
            _context = new CvgsDbContext(options);

            // Setup the AdminController with mocked dependencies
            _adminController = new AdminController(_context, _mockHostEnv.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task AddGame_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var invalidGameModel = new GameViewModel
            {
                // Missing required fields
                Description = "Incomplete data"
            };

            // Mock model state validation failure
            _adminController.ModelState.AddModelError("Title", "The Title field is required.");

            // Act
            var result = await _adminController.AddGame(invalidGameModel);

            // Assert
            var viewResult = result as ViewResult;
            Assert.AreEqual(false, viewResult.ViewData.ModelState.IsValid);
        }

        [Test]
        public async Task DeleteGame_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            // Act
            var result = await _adminController.DeleteGame(999);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

    }
}
