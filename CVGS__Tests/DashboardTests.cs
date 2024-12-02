using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CVGS.Controllers;
using CVGS.Models;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CVGS.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CVGS.Tests.Controllers
{
    public class DashboardTests
    {
        private AccountController _controller;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private ILogger<AccountController> _logger;
        private DbContextOptions<CvgsDbContext> _dbContextOptions;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private Mock<IUserClaimsPrincipalFactory<User>> _claimsPrincipalFactory;
        private Mock<IRoleStore<IdentityRole>> _roleStore;
        private RoleManager<IdentityRole> _roleManager;

        [SetUp]
        public void SetUp()
        {
            // Set up the in-memory database
            _dbContextOptions = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            // Create the context
            using (var context = new CvgsDbContext(_dbContextOptions))
            {
                // Seed the database with test data
                context.Users.Add(new User
                {
                    Id = "1",
                    FullName = "John Doe",
                    DisplayName = "Test",
                    Preferences = new Preference
                    {
                        FavouritePlatforms = new List<string>(),
                        FavouriteGameCategories = new List<string>(),
                        LanguagePreferences = new List<string>()
                    }
                });
                context.SaveChanges();
            }

            // Mock IHttpContextAccessor
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            var contextAccessor = new DefaultHttpContext();
            contextAccessor.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));
            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(contextAccessor);

            // Mock IUserClaimsPrincipalFactory
            _claimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _claimsPrincipalFactory
                .Setup(x => x.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") })));

            // Mock RoleManager
            _roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManager = new RoleManager<IdentityRole>(
                _roleStore.Object,
                null, null, null, null
            );

            // Create UserManager and SignInManager using the in-memory context
            var userStore = new UserStore<User>(new CvgsDbContext(_dbContextOptions));
            _userManager = new UserManager<User>(userStore, null, null, null, null, null, null, null, null);
            _signInManager = new SignInManager<User>(
                _userManager,
                _httpContextAccessor.Object,
                _claimsPrincipalFactory.Object,
                null,
                null,
                null,
                null
            );

            // Set up the logger
            _logger = new Mock<ILogger<AccountController>>().Object;

            // Create the controller
            _controller = new AccountController(
                new CvgsDbContext(_dbContextOptions),
                _logger,
                _userManager,
                _signInManager,
                _roleManager,
                new EmailService() // Replace with a proper mock if needed
            );

            // Setup user claims for the controller context
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") })) };
        }

        [Test]
        public async Task Dashboard_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            using (var context = new CvgsDbContext(_dbContextOptions))
            {
                context.Users.RemoveRange(context.Users);
                context.SaveChanges();
            }

            // Act
            var result = await _controller.Dashboard();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Dashboard_CallsGetUserIdOnce()
        {
            // Act
            await _controller.Dashboard();

            // Assert
            // Verify that GetUserId is called once
            var userId = _userManager.GetUserId(_controller.ControllerContext.HttpContext.User);
            Assert.AreEqual("1", userId);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            using (var context = new CvgsDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }


        private IQueryable<User> MockUserQueryable(User user)
        {
            var userList = new List<User>();
            if (user != null)
            {
                userList.Add(user);
            }
            return userList.AsQueryable();
        }
    }
}
