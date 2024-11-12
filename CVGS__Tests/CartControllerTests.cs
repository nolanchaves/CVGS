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
    public class CartControllerTests
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

        [Test]
        public async Task RemoveFromCart_RemovesItemFromCart()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), DisplayName = "Test User", Email = "testuser@example.com" }; // Unique ID
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _context.Users.Add(user);
            var game = new Game { GameID = 2, Title = "Test Game", Price = 10.0m, Category = "Shooter", CoverImageURL = "/images/testimage.jpg", Description = "Test Game Description", LanguageSupport = "English", Platform = "PC" };
            _context.Games.Add(game);
            var cart = new Cart { UserID = user.Id, CartItems = new List<CartItem> { new CartItem { GameId = 1, Quantity = 1 } } };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.RemoveFromCart(1);

            // Assert
            var updatedCart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserID == user.Id);
            Assert.IsNotNull(updatedCart);
            Assert.IsEmpty(updatedCart.CartItems);
        }

        [Test]
        public async Task ClearCart_UserNotLoggedIn_RedirectsToLogin()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.ClearCart();

            // Assert
            var redirectToLogin = result as RedirectToActionResult;
            Assert.IsNotNull(redirectToLogin);
            Assert.AreEqual("Login", redirectToLogin.ActionName);
        }


        [Test]
        public async Task GetCartItemCount_UserNotLoggedIn_ReturnsZero()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((User)null);

            // Act
            var count = await _controller.GetCartItemCount();

            // Assert
            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task GetCartItemCount_ReturnsCorrectCount()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), DisplayName = "Test User", Email = "testuser@example.com" };
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _context.Users.Add(user);
            var cart = new Cart
            {
                UserID = user.Id,
                CartItems = new List<CartItem>
            {
                new CartItem { GameId = 1, Quantity = 2 },
                new CartItem { GameId = 2, Quantity = 3 }
            }
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            // Act
            var count = await _controller.GetCartItemCount();

            // Assert
            Assert.AreEqual(5, count); // Total quantity is 2 + 3
        }
    }
}