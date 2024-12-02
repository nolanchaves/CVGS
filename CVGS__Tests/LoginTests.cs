using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using CVGS.Controllers;
using CVGS.Entities;
using CVGS.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;

namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class LoginTests
    {
        private AccountController _controller;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;

        [SetUp]
        public void SetUp()
        {
            // Mock UserManager
            var userStore = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

            // Mock RoleManager
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            // Mock HttpContext
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }));

            // Mock the DbContext with In-Memory Database
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var dbContext = new CvgsDbContext(options);

            // Initialize the controller with mocked dependencies
            _controller = new AccountController(
                dbContext,
                new Mock<ILogger<AccountController>>().Object,
                _userManagerMock.Object,
                new SignInManager<User>(
                    _userManagerMock.Object,
                    new Mock<IHttpContextAccessor>().Object,
                    new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                    null, null, null, null
                ),
                _roleManagerMock.Object,
                new Mock<EmailService>().Object
            )
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
        }


        [Test]
        public void Login_Get_UserIsNotAuthenticated_ReturnsView()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act
            var result = _controller.Login();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Login_Post_InvalidModelState_ReturnsViewWithErrors()
        {
            // Arrange
            var model = new LoginViewModel { DisplayName = "user", Password = "" }; // Empty password causes invalid model state
            _controller.ModelState.AddModelError("Password", "Password is required");

            // Act
            var result = await _controller.Login(model);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult); // Ensure it's a ViewResult
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual(model, viewResult?.Model); // Assert the model is returned
        }

    }
}