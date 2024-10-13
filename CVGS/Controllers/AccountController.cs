using Microsoft.AspNetCore.Mvc;
using CVGS.Models;
using CVGS.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CVGS.Entities.CVGS.Entities;
using System.Data;
using NETCore.MailKit.Core;
using MailKit.Net.Smtp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Numerics;
using Microsoft.IdentityModel.Tokens;

namespace CVGS.Controllers
{
    public class AccountController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailService _emailService;


        public AccountController(CvgsDbContext context, ILogger<AccountController> logger, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, EmailService emailService)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            string captchaResponse = Request.Form["g-recaptcha-response"].ToString();
            Debug.WriteLine($"Captcha Response: {captchaResponse}");

            var client = new HttpClient();
            var secretKey = "6LeotlsqAAAAAOu3Z0Ng9j1M8x67O4t0nnhy2p6l";
            var captchaResult = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}");
            dynamic jsonData = JsonConvert.DeserializeObject(captchaResult);

            if (jsonData.success != true)
            {
                ModelState.AddModelError("CaptchaError", "reCAPTCHA validation failed. Please try again.");
                return View(model);
            }

            User existingUser = await _userManager.FindByNameAsync(model.DisplayName);
            User existingEmail = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("DisplayName", "Username already exists.");
            }

            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email already exists.");
            }

            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    DisplayName = model.DisplayName,
                    UserName = model.DisplayName,
                    Email = model.Email,
                    EmailConfirmed = false
                };

                Debug.WriteLine("Creating user...");

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Debug.WriteLine($"User created successfully: {user.UserName}");
                    TempData["SuccessMessage"] = "Account was created successfully! \nPlease verify your account in your email";
                    var roleExists = await _roleManager.RoleExistsAsync("User");
                    if (roleExists)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, "User");

                        if (result.Succeeded)
                        {
                            Debug.WriteLine($"User {user.UserName} assigned to User role.");
                        }
                        else
                        {
                            Debug.WriteLine($"Failed to assign {user.UserName} to User role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("User role does not exist.");
                    }

                    await SendValidationEmailAsync(user.Email, user.Id);

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    Debug.WriteLine("User creation failed. Errors:");
                    foreach (IdentityError error in result.Errors)
                    {
                        Debug.WriteLine($"Error: {error.Description}");
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.DisplayName);
                if (user != null)
                {
                    Debug.WriteLine($"Log in attempt");

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError(string.Empty, "Please confirm your new account in your email. \nIf not found please check you junk mail!");
                    }
                    else
                    {
                        if (result.Succeeded)
                        {
                            Debug.WriteLine($"User logged in successfully: {user.UserName}");
                            HttpContext.Session.Remove("LoginAttempts");

                            var roles = await _userManager.GetRolesAsync(user);
                            Debug.WriteLine($"User roles: {string.Join(", ", roles)}");

                            if (roles.Contains("Admin"))
                            {
                                Debug.WriteLine("Redirecting to Admin Panel.");
                                return RedirectToAction("Panel", "Account");
                            }
                            else
                            {
                                Debug.WriteLine("Redirecting to User Dashboard.");
                                return RedirectToAction("Dashboard", "Account");
                            }
                        }
                        else if (result.IsLockedOut)
                        {
                            ModelState.AddModelError(string.Empty, "Account locked due to too many failed attempts. Please try again later.");
                            Debug.WriteLine("Account locked due to too many failed attempts.");
                            return View(model);
                        }
                        else
                        {
                            int attempts = HttpContext.Session.GetInt32("LoginAttempts") ?? 0;
                            attempts++;
                            HttpContext.Session.SetInt32("LoginAttempts", attempts);
                            Debug.WriteLine($"User attempt: {attempts}");

                            if (attempts >= 3)
                            {
                                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(15));
                                ModelState.AddModelError(string.Empty, "Account locked due to too many failed attempts. Please try again later.");
                                Debug.WriteLine("Account locked due to too many failed attempts.");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            }
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            User user = await _userManager.Users
                .Include(u => u.Address)
                .Include(u => u.ShippingAddress) // Add this line if ShippingAddress is a separate entity
                .Include(u => u.Preferences)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));


            if (user == null)
            {
                Debug.WriteLine("User not found");
                return NotFound();
            }

            ProfileViewModel model = new ProfileViewModel
            {
                ActualName = user.FullName ?? "N/A",
                Gender = user.Gender ?? "N/A",
                BirthDate = user.BirthDate.HasValue
                            ? (DateOnly)user.BirthDate.Value
                            : DateOnly.MinValue,

                ReceivePromotionalEmails = user.ReceivePromotionalEmails ?? false,

                Preferences = new PreferenceViewModel
                {
                    FavouritePlatforms = user.Preferences?.FavouritePlatforms ?? new List<string>(),
                    FavouriteGameCategories = user.Preferences?.FavouriteGameCategories ?? new List<string>(),
                    LanguagePreferences = user.Preferences?.LanguagePreferences ?? new List<string>()
                },

                Address = new AddressViewModel
                {
                    FullName = user.FullName ?? "N/A",
                    PhoneNumber = user.Address?.PhoneNumber ?? "N/A",
                    StreetAddress = user.Address?.StreetAddress ?? "N/A",
                    AptSuite = user.Address?.AptSuite ?? "N/A",
                    City = user.Address?.City ?? "N/A",
                    Province = user.Address?.Province ?? "N/A",
                    PostalCode = user.Address?.PostalCode ?? "N/A",
                    Country = user.Address?.Country ?? "N/A",
                    DeliveryInstructions = user.Address?.DeliveryInstructions ?? "N/A",
                    SameAsShippingAddress = user.Address?.SameAsShippingAddress ?? false,
                    ShippingFullName = user.FullName,
                    ShippingPhoneNumber = user.ShippingAddress?.ShippingPhoneNumber ?? "N/A",
                    ShippingStreetAddress = user.ShippingAddress?.ShippingStreetAddress ?? "N/A",
                    ShippingAptSuite = user.ShippingAddress?.ShippingAptSuite ?? "N/A",
                    ShippingCity = user.ShippingAddress?.ShippingCity ?? "N/A",
                    ShippingProvince = user.ShippingAddress?.ShippingProvince ?? "N/A",
                    ShippingPostalCode = user.ShippingAddress?.ShippingPostalCode ?? "N/A",
                    ShippingCountry = user.ShippingAddress?.ShippingCountry ?? "N/A"
                }
            };

            Debug.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            User user = await _userManager.Users
                .Include(u => u.Preferences)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel model = new ProfileViewModel
            {
                ActualName = user.FullName ?? "",
                Gender = user.Gender ?? "",
                BirthDate = user.BirthDate != null ? (DateOnly)user.BirthDate : DateOnly.MinValue,
                ReceivePromotionalEmails = user.ReceivePromotionalEmails ?? false,
                FavouritePlatforms = user.Preferences?.FavouritePlatforms ?? new List<string>(),
                FavouriteGameCategories = user.Preferences?.FavouriteGameCategories ?? new List<string>(),
                LanguagePreferences = user.Preferences?.LanguagePreferences ?? new List<string>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            // Retrieve the current user
            User user = await _userManager.Users
                .Include(u => u.Preferences)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.FullName = model.ActualName;
                user.Gender = model.Gender;
                user.BirthDate = model.BirthDate;
                user.ReceivePromotionalEmails = model.ReceivePromotionalEmails;

                await _context.SaveChangesAsync();

                return RedirectToAction("Dashboard", "Account");
            }

            return View(model);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Preferences()
        {
            var user = await _userManager.Users
                .Include(u => u.Preferences)
                .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user == null)
            {
                return NotFound();
            }

            if (user.Preferences == null)
            {
                user.Preferences = new Preference
                {
                    FavouritePlatforms = new List<string>(),
                    FavouriteGameCategories = new List<string>(),
                    LanguagePreferences = new List<string>()
                };
            }

            PreferenceViewModel model = new PreferenceViewModel
            {
                FavouritePlatforms = user.Preferences.FavouritePlatforms ?? new List<string>(),
                FavouriteGameCategories = user.Preferences.FavouriteGameCategories ?? new List<string>(),
                LanguagePreferences = user.Preferences.LanguagePreferences ?? new List<string>(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Preferences(PreferenceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var existingPreferences = await _context.Preferences
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                if (existingPreferences == null)
                {
                    existingPreferences = new Preference
                    {
                        UserId = user.Id
                    };


                    await _context.Preferences.AddAsync(existingPreferences);
                }

                existingPreferences.FavouritePlatforms = model.FavouritePlatforms;
                existingPreferences.FavouriteGameCategories = model.FavouriteGameCategories;
                existingPreferences.LanguagePreferences = model.LanguagePreferences;

                if (user.PreferenceId == null)
                {
                    user.PreferenceId = existingPreferences.PreferenceId;
                }

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Preferences updated successfully!";
                    return RedirectToAction("Dashboard", "Account");
                }

                ModelState.AddModelError(string.Empty, "Failed to update preferences.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Address()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var existingAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == user.Id);

                var shippingAddress = await _context.ShippingAddresses
                    .FirstOrDefaultAsync(s => s.UserId == user.Id);

                var model = new AddressViewModel
                {
                    //Address
                    FullName = user.FullName,
                    PhoneNumber = existingAddress?.PhoneNumber,
                    StreetAddress = existingAddress?.StreetAddress,
                    AptSuite = existingAddress?.AptSuite,
                    City = existingAddress?.City,
                    Province = existingAddress?.Province,
                    PostalCode = existingAddress?.PostalCode,
                    Country = existingAddress?.Country,
                    DeliveryInstructions = existingAddress?.DeliveryInstructions,
                    SameAsShippingAddress = existingAddress?.SameAsShippingAddress ?? false,

                    //Shipping Address
                    ShippingFullName = user.FullName,
                    ShippingStreetAddress = shippingAddress?.ShippingStreetAddress,
                    ShippingAptSuite = shippingAddress?.ShippingAptSuite,
                    ShippingCity = shippingAddress?.ShippingCity,
                    ShippingProvince = shippingAddress?.ShippingProvince,
                    ShippingPostalCode = shippingAddress?.ShippingPostalCode,
                    ShippingCountry = shippingAddress?.ShippingCountry,
                    ShippingPhoneNumber = shippingAddress?.ShippingPhoneNumber
                };

                return View(model);
            }

            ModelState.AddModelError(string.Empty, "User not found.");
            return View(new AddressViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address(AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var existingAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == user.Id);

                if (existingAddress == null)
                {
                    existingAddress = new Address
                    {
                        UserId = user.Id,
                        SameAsShippingAddress = model.SameAsShippingAddress,
                        StreetAddress = model.StreetAddress,
                        AptSuite = model.AptSuite,
                        City = model.City,
                        Province = model.Province,
                        PostalCode = model.PostalCode.Trim(),
                        Country = model.Country,
                        DeliveryInstructions = model.DeliveryInstructions,
                        PhoneNumber = model.PhoneNumber
                    };
                    _context.Add(existingAddress);
                }
                else
                {
                    existingAddress.SameAsShippingAddress = model.SameAsShippingAddress;
                    existingAddress.StreetAddress = model.StreetAddress;
                    existingAddress.AptSuite = model.AptSuite;
                    existingAddress.City = model.City;
                    existingAddress.Province = model.Province;
                    existingAddress.PostalCode = model.PostalCode.Trim();
                    existingAddress.Country = model.Country;
                    existingAddress.DeliveryInstructions = model.DeliveryInstructions;
                    existingAddress.PhoneNumber = model.PhoneNumber;
                }

                var shippingAddress = await _context.ShippingAddresses
                    .FirstOrDefaultAsync(a => a.UserId == user.Id) ?? new ShippingAddress { UserId = user.Id };

                if (model.SameAsShippingAddress)
                {
                    shippingAddress.ShippingStreetAddress = existingAddress.StreetAddress;
                    shippingAddress.ShippingAptSuite = existingAddress.AptSuite;
                    shippingAddress.ShippingCity = existingAddress.City;
                    shippingAddress.ShippingProvince = existingAddress.Province;
                    shippingAddress.ShippingPostalCode = existingAddress.PostalCode.Trim();
                    shippingAddress.ShippingCountry = existingAddress.Country;
                    shippingAddress.ShippingPhoneNumber = existingAddress.PhoneNumber;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.ShippingStreetAddress))
                    {
                        ModelState.AddModelError(nameof(model.ShippingStreetAddress), "Shipping street address is required.");
                    }
                    if (string.IsNullOrWhiteSpace(model.ShippingCity))
                    {
                        ModelState.AddModelError(nameof(model.ShippingCity), "Shipping city is required.");
                    }
                    if (string.IsNullOrWhiteSpace(model.ShippingProvince))
                    {
                        ModelState.AddModelError(nameof(model.ShippingProvince), "Shipping province is required.");
                    }
                    if (string.IsNullOrWhiteSpace(model.ShippingPostalCode))
                    {
                        ModelState.AddModelError(nameof(model.ShippingPostalCode), "Shipping postal code is required.");
                    }
                    if (string.IsNullOrWhiteSpace(model.ShippingCountry))
                    {
                        ModelState.AddModelError(nameof(model.ShippingCountry), "Shipping country is required.");
                    }
                    if (string.IsNullOrWhiteSpace(model.ShippingPhoneNumber))
                    {
                        ModelState.AddModelError(nameof(model.ShippingCountry), "Shipping phone number is required.");
                    }

                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }

                    // Trim whitespace from ShippingPostalCode
                    shippingAddress.ShippingPostalCode = model.ShippingPostalCode?.Trim();
                    shippingAddress.ShippingStreetAddress = model.ShippingStreetAddress;
                    shippingAddress.ShippingAptSuite = model.ShippingAptSuite;
                    shippingAddress.ShippingCity = model.ShippingCity;
                    shippingAddress.ShippingProvince = model.ShippingProvince;
                    shippingAddress.ShippingCountry = model.ShippingCountry;
                    shippingAddress.ShippingPhoneNumber = model.ShippingPhoneNumber;
                }

                if (shippingAddress.ShippingAddressId == 0)
                {
                    _context.Add(shippingAddress);
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Address updated successfully!";
                    return RedirectToAction("Dashboard", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            return View(model);
        }


        [Authorize(Policy = "Admin")]
        public IActionResult Panel()
        {
            return View();
        }


        private async Task SendValidationEmailAsync(string email, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var validationLink = Url.Action("ValidateEmail", "Account", new { userId = userId, token = token }, Request.Scheme);
            var subject = "Email Validation";
            var body = $@"<p>Thank you for signing up!</p>
            <p>Please validate your email by clicking {validationLink} here</a> to verify your account.</p>
            <p>If you did not sign up, please ignore this email.</p>
            <p>Best regards,<br>Conestoga Virtual Game Store</p>";
            var emailService = new EmailService();
            await emailService.SendEmailAsync(email, subject, body);
        }


        public async Task<IActionResult> ValidateEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return View("EmailValidationFailed");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return View("EmailValidationSuccess");
                }
            }

            return View("EmailValidationFailed");
        }

    }
}
