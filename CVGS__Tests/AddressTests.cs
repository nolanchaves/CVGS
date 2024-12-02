using CVGS;
using CVGS.Controllers;
using CVGS.Entities;
using CVGS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CVGS.Tests.Controllers
{
    [TestFixture]
    public class AddressTests
    {
        private CvgsDbContext _context;
        private Mock<ILogger<AccountController>> _mockLogger;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<SignInManager<User>> _mockSignInManager;
        private Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private Mock<EmailService> _mockEmailService;
        private AccountController _controller;

        [SetUp]
        public void SetUp()
        {
            // Mock DbContext with InMemoryDatabase
            var options = new DbContextOptionsBuilder<CvgsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CvgsDbContext(options);

            // Mock UserManager
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            // Mock SignInManager
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var mockClaimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            var mockIdentityOptions = new Mock<IOptions<IdentityOptions>>();
            var mockLoggerSignInManager = new Mock<ILogger<SignInManager<User>>>();
            var mockAuthSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
            var mockUserConfirmation = new Mock<IUserConfirmation<User>>();

            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object,
                mockContextAccessor.Object,
                mockClaimsFactory.Object,
                mockIdentityOptions.Object,
                mockLoggerSignInManager.Object,
                mockAuthSchemeProvider.Object,
                mockUserConfirmation.Object
            );

            // Mock RoleManager
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            // Mock EmailService
            _mockEmailService = new Mock<EmailService>();

            // Mock Logger for AccountController
            _mockLogger = new Mock<ILogger<AccountController>>();

            // Create the controller with all mocked dependencies
            _controller = new AccountController(_context, _mockLogger.Object, _mockUserManager.Object, _mockSignInManager.Object, _mockRoleManager.Object, _mockEmailService.Object);
        }

        [Test]
        public async Task Address_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("PhoneNumber", "Required");

            // Act
            var result = await _controller.Address(new AddressViewModel());

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public async Task Address_Get_UserFound_ReturnsViewWithModel()
        {
            // Arrange
            var userId = "user1";
            var mockUser = new User { Id = userId };
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);

            var mockAddress = new Address
            {
                UserId = userId,
                PhoneNumber = "1234567890",
                StreetAddress = "Test Street",
                City = "TestCity",
                Country = "TestCountry",
                PostalCode = "n1r5s3",
                Province = "TestProvince"
            };

            var mockShippingAddress = new ShippingAddress
            {
                UserId = userId,
                ShippingStreetAddress = "Shipping Street",
                ShippingCity = "ShippingCity",
                ShippingCountry = "ShippingCountry",
                ShippingPhoneNumber = "0987654321",
                ShippingPostalCode = "a1b2c3",
                ShippingProvince = "ShippingProvince"
            };

            _context.Addresses.Add(mockAddress);
            _context.ShippingAddresses.Add(mockShippingAddress);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Address();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as AddressViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual("Test Street", model.StreetAddress);
            Assert.AreEqual("Shipping Street", model.ShippingStreetAddress);
            Assert.AreEqual("ShippingCity", model.ShippingCity);
            Assert.AreEqual("ShippingCountry", model.ShippingCountry);
            Assert.AreEqual("0987654321", model.ShippingPhoneNumber);
            Assert.AreEqual("a1b2c3", model.ShippingPostalCode);
            Assert.AreEqual("ShippingProvince", model.ShippingProvince);
        }



    }
}
