using CVGS.Controllers;
using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class GameTests
    {
        private CvgsDbContext _context;
        private Mock<UserManager<User>> _mockUserManager;
        private GamesController _controller;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CvgsDbContext(options);
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _controller = new GamesController(_context, null, _mockUserManager.Object); // Pass null for ReviewService
        }

        [Test]
        public void GameDetails_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = _controller.GameDetails(999) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GameDetails_NoGame_ReturnsNotFound()
        {
            // Act
            var result = _controller.GameDetails(1) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}