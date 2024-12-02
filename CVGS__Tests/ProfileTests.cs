using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CVGS.Controllers;
using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class ProfileTests
    {
        private CvgsDbContext _dbContext;
        private AccountController _controller;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<SignInManager<User>> _signInManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;

        [SetUp]
        public void SetUp()
        {
            // Configure an in-memory database for EF Core
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database for each test
                .Options;

            _dbContext = new CvgsDbContext(options);

            // Mock UserManager
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object,
                null,
                null,
                Array.Empty<IUserValidator<User>>(),
                Array.Empty<IPasswordValidator<User>>(),
                null,
                null,
                null,
                null
            );

            // Mock SignInManager
            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null
            );

            // Mock RoleManager
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object,
                Array.Empty<IRoleValidator<IdentityRole>>(),
                null,
                null,
                null
            );

            // Initialize the controller
            _controller = new AccountController(
                _dbContext,
                new Mock<ILogger<AccountController>>().Object,
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _roleManagerMock.Object,
                new Mock<EmailService>().Object
            );

            // Mock HttpContext and user claims
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "123")
            }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }


        [Test]
        public async Task Profile_Get_UserFound_ReturnsViewWithCorrectModel()
        {
            // Arrange
            var user = new User
            {
                Id = "123",
                FullName = "John Doe",
                DisplayName = "Test",
                Gender = "Male",
                BirthDate = DateOnly.Parse("1990-05-05"),
                ReceivePromotionalEmails = true
            };

            // Add the user to the in-memory database
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.Profile();

            // Assert
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult); // Verify the action returns a ViewResult

            var model = viewResult?.Model as ProfileViewModel;
            Assert.NotNull(model); // Verify the model is of the expected type
        }
    }
}
