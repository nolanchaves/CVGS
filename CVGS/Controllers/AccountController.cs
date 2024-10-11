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
            // Validate reCAPTCHA
            string captchaResponse = Request.Form["g-recaptcha-response"].ToString();

            // Check if the reCAPTCHA response is empty
            if (string.IsNullOrEmpty(captchaResponse))
            {
                ModelState.AddModelError("", "reCAPTCHA is required"); // Specific error for Captcha
                return View(model);
            }

            bool isCaptchaValid = await ValidateCaptcha(captchaResponse);
            if (!isCaptchaValid)
            {
                ModelState.AddModelError("", "reCAPTCHA validation failed. Please try again."); // Specific error for Captcha
                return View(model);
            }

            // Check if user already exists
            User existingUser = await _userManager.FindByNameAsync(model.DisplayName);
            User existingEmail = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null && existingEmail != null)
            {
                ModelState.AddModelError("DisplayName", "Username already exists.");
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }
            else if (existingUser != null)
            {
                ModelState.AddModelError("DisplayName", "Username already exists.");
                return View(model);
            }
            else if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            // Adds new valid user
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
                    // Use SignInManager to sign in
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Reset the attempt counter
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
                        // Increment the attempt counter
                        int attempts = HttpContext.Session.GetInt32("LoginAttempts") ?? 0;
                        attempts++;
                        HttpContext.Session.SetInt32("LoginAttempts", attempts);
                        Debug.WriteLine($"User attempt: {attempts}");

                        if (attempts >= 3)
                        {
                            // Lock out the user
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(15)); // Lock for 15 minutes
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            User user = await _userManager.Users
                    .Include(u => u.Address)
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
                    FavouritePlatforms = user.FavouritePlatforms ?? new List<string>(),
                    FavouriteGameCategories = user.FavouriteGameCategories ?? new List<string>(), 
                    LanguagePreferences = user.LanguagePreferences ?? new List<string>()
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
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                ActualName = user.FullName ?? "N/A",
                Gender = user.Gender ?? "N/A",
                BirthDate = (DateOnly)(user.BirthDate != null ? user.BirthDate : DateOnly.MinValue),
                ReceivePromotionalEmails = (bool)user.ReceivePromotionalEmails,
                FavoritePlatforms = user.FavouritePlatforms,
                FavoriteGameCategories = user.FavouriteGameCategories,
                LanguagePreferences = user.LanguagePreferences
            };




            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    user.FullName = model.ActualName;
                    user.Gender = model.Gender;
                    user.BirthDate = model.BirthDate;
                    user.ReceivePromotionalEmails = model.ReceivePromotionalEmails;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Profile updated successfully!";
                        return RedirectToAction("Dashboard");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Preferences()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }
            var model = new PreferenceViewModel
            {
                FavouritePlatforms = user.FavouritePlatforms ?? new List<string>(),
                FavouriteGameCategories = user.FavouriteGameCategories ?? new List<string>(),
                LanguagePreferences = user.LanguagePreferences ?? new List<string>()
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

                user.FavouritePlatforms = model.FavouritePlatforms;
                user.FavouriteGameCategories = model.FavouriteGameCategories;
                user.LanguagePreferences = model.LanguagePreferences;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Preferences updated successfully!";
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Address()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new AddressViewModel
            {
                FullName = user.FullName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                StreetAddress = user.Address?.StreetAddress ?? string.Empty,
                AptSuite = user.Address?.AptSuite ?? string.Empty,
                City = user.Address?.City ?? string.Empty,
                Province = user.Address?.Province ?? string.Empty,
                PostalCode = user.Address?.PostalCode ?? string.Empty,
                Country = user.Address?.Country ?? string.Empty,
                DeliveryInstructions = user.Address?.DeliveryInstructions ?? string.Empty,
                SameAsShippingAddress = user.SameAsShippingAddress ?? false
            };
            return View(model);
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
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;

                if (user.Address == null)
                {
                    user.Address = new Address();
                }

                user.Address.StreetAddress = model.StreetAddress;
                user.Address.AptSuite = model.AptSuite;
                user.Address.City = model.City;
                user.Address.Province = model.Province;
                user.Address.PostalCode = model.PostalCode;
                user.Address.Country = model.Country;
                user.Address.DeliveryInstructions = model.DeliveryInstructions;
                user.SameAsShippingAddress = model.SameAsShippingAddress;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Preferences updated successfully!";
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


        private async Task<bool> ValidateCaptcha(string captchaResponse)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent($"secret={"6LeotlsqAAAAAOu3Z0Ng9j1M8x67O4t0nnhy2p6l"}&response={captchaResponse}", Encoding.UTF8, "application/x-www-form-urlencoded");
                var result = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var jsonResult = await result.Content.ReadAsStringAsync();
                var captchaResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(jsonResult);
                return captchaResult.Success;
            }
        }

        private class GoogleCaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("challenge_ts")]
            public string ChallengeTimeStamp { get; set; }

            [JsonProperty("hostname")]
            public string HostName { get; set; }

            [JsonProperty("score")]
            public float Score { get; set; }

            [JsonProperty("action")]
            public string Action { get; set; }
        }

    }
}
