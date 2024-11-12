using CVGS;
using CVGS.Controllers;
using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace CVGS.Tests.Controllers
{
    [TestFixture]
public class ReviewControllerTests
    {
        private CvgsDbContext _context;
        private Mock<UserManager<User>> _mockUserManager;
        private ReviewController _controller;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new CvgsDbContext(options);
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _controller = new ReviewController(_context, _mockUserManager.Object);
        }



        [Test]
        public void AddReview_Get_UnauthorizedUser_RedirectsToLogin()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = _controller.AddReview(1);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Login", (result as RedirectToActionResult).ActionName);
        }



        [Test]
        public async Task AddReview_Post_NonExistentGame_ReturnsNotFound()
        {
            // Arrange
            var user = new User { Id = "user123" };
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(user.Id);
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var model = new ReviewRateViewModel { GameId = 99, Rate = 5, Review = "Great!" }; // Game ID 99 does not exist

            // Act
            var result = await _controller.AddReview(model);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GameDetails", (result as RedirectToActionResult).ActionName);
        }
    }
}