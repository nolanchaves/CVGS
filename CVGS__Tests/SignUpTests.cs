using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using CVGS.Controllers;
using CVGS.Entities;
using CVGS.Models;
using CVGS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using NETCore.MailKit.Core;

namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class SignUpTests
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
        public void SignUp_Get_ReturnsView()
        {
            // Act
            var result = _controller.SignUp();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.IsNull(viewResult?.ViewName);
        }

        [Test]
        public async Task SignUp_Post_EmailAlreadyExists_ReturnsViewWithErrors()
        {
            // Arrange
            var model = new SignUpViewModel
            {
                DisplayName = "testuser",
                Email = "existing@example.com",
                Password = "Password123!",
            };

            // Mock the UserManager to simulate existing email
            _userManagerMock.Setup(x => x.FindByNameAsync(model.DisplayName)).ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.FindByEmailAsync(model.Email)).ReturnsAsync(new User()); // Simulate existing email

            // Act
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "DisplayName", model.DisplayName },
                { "Email", model.Email },
                { "Password", model.Password }
            });

            var result = await _controller.SignUp(model);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsFalse(_controller.ModelState.IsValid);
        }
    }
}