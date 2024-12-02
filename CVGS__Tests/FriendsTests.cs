using CVGS.Controllers;
using CVGS.Entities;
using CVGS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class FriendsTests
    {
        private CvgsDbContext _context;
        private Mock<UserManager<User>> _mockUserManager;
        private FriendsController _controller;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new CvgsDbContext(options);

            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _controller = new FriendsController(_context, _mockUserManager.Object);
        }

        [Test]
        public async Task AddFriend_InvalidUserId_RedirectToFriendsList()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), DisplayName = "Test User", Email = "testuser@example.com" }; // Unique ID
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.AddFriend("999",999);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("FriendsList", (result as RedirectToActionResult).ActionName);
        }
        
        [Test]
        public async Task RemoveRequest_InvalidId_RedirectToFriendsList()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid().ToString(), DisplayName = "Test User", Email = "testuser@example.com" }; // Unique ID
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.RemoveRequest(999);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("FriendsList", (result as RedirectToActionResult).ActionName);
        }
    }
}
