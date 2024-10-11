using Microsoft.AspNetCore.Mvc;
using CVGS.Models;
using CVGS.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using CVGS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CVGS.Entities.CVGS.Entities;

namespace CVGS.Controllers
{
    public class AccountController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(CvgsDbContext context, ILogger<AccountController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
                    Role = "User",
                    EmailConfirmed = true
                };

                Debug.WriteLine("Creating user...");

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Debug.WriteLine($"User created successfully: {user.UserName}");
                    // Optionally send a confirmation email here


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
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        Debug.WriteLine($"User logged in successfully: {user.UserName}");
                        HttpContext.Session.Remove("LoginAttempts");
                        return RedirectToAction("Dashboard", "Account");
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
                    PhoneNumber = user.PhoneNumber ?? "N/A",
                    StreetAddress = user.Address?.StreetAddress ?? "N/A",
                    AptSuite = user.Address?.AptSuite ?? "N/A",
                    City = user.Address?.City ?? "N/A",
                    Province = user.Address?.Province ?? "N/A",
                    PostalCode = user.Address?.PostalCode ?? "N/A",
                    Country = user.Address?.Country ?? "N/A",
                    DeliveryInstructions = user.Address?.DeliveryInstructions ?? "N/A",
                    SameAsShippingAddress = user.SameAsShippingAddress ?? false
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
                ActualName = user.FullName ?? "N/A",
                Gender = user.Gender ?? "N/A",
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
            User user = await _userManager.Users
                .Include(u => u.Preferences)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.ActualName;
            user.Gender = model.Gender;
            user.BirthDate = model.BirthDate;
            user.ReceivePromotionalEmails = model.ReceivePromotionalEmails;

            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard", "Account");
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
                // Fetch the existing address for the user
                var existingAddress = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == user.Id);

                // Create the model with user and address details
                var model = new AddressViewModel
                {
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    StreetAddress = existingAddress?.StreetAddress,
                    AptSuite = existingAddress?.AptSuite,
                    City = existingAddress?.City,
                    Province = existingAddress?.Province,
                    PostalCode = existingAddress?.PostalCode,
                    Country = existingAddress?.Country,
                    DeliveryInstructions = existingAddress?.DeliveryInstructions,
                    SameAsShippingAddress = user.SameAsShippingAddress ?? false // Default to false if null
                };

                return View(model);
            }

            // Handle case where user is not found
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
                var existingAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == user.Id);

                if (existingAddress == null)
                {
                    existingAddress = new Address
                    {
                        UserId = user.Id 
                    };
                    _context.Add(existingAddress);
                }

                existingAddress.StreetAddress = model.StreetAddress;
                existingAddress.AptSuite = model.AptSuite;
                existingAddress.City = model.City;
                existingAddress.Province = model.Province;
                existingAddress.PostalCode = model.PostalCode;
                existingAddress.Country = model.Country;
                existingAddress.DeliveryInstructions = model.DeliveryInstructions;


                if (user.AddressId == null)
                {
                    user.AddressId = existingAddress.AddressId;
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
    }
}
