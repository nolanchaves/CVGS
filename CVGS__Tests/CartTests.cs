using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using CVGS.Controllers;
using CVGS.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CVGS;

namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class CartTests
    {
        private CvgsDbContext _context;
        private Mock<UserManager<User>> _mockUserManager;
        private CartController _controller;

        [SetUp]
        public void SetUp()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new CvgsDbContext(options);

            // Set up UserManager dependencies
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new CartController(_context, _mockUserManager.Object);
        }

        [Test]
        public async Task ViewCart_UserHasNoCart_ReturnsEmptyView()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), DisplayName = "Test User", Email = "testuser@example.com" }; // Unique ID
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.ViewCart();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.Model); // Assuming the model is null when there's no cart
        }

        [Test]
        public async Task RemoveFromCart_UserNotLoggedIn_RedirectsToLogin()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.RemoveFromCart(1);

            // Assert
            var redirectToLogin = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToLogin);
            Assert.AreEqual("Login", redirectToLogin.ActionName);
        }
    }
}