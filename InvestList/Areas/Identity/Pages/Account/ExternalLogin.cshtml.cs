#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Radar.Domain.Entities;

namespace InvestList.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            IConfiguration config,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        // Bind Telegram callback parameters
        [BindProperty(SupportsGet = true)] public string Provider { get; set; }
        [BindProperty(SupportsGet = true)] public string Id { get; set; }
        [BindProperty(SupportsGet = true)] public string FirstName { get; set; }
        [BindProperty(SupportsGet = true)] public string Username { get; set; }
        [BindProperty(SupportsGet = true)] public string PhotoUrl { get; set; }
        [BindProperty(SupportsGet = true)] public string AuthDate { get; set; }
        [BindProperty(SupportsGet = true)] public string Hash { get; set; }

        public string ProviderDisplayName { get; set; }
        public string ReturnUrl { get; set; }
        [TempData] public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { provider, returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            var culture = HttpContext.Request.RouteValues["culture"] as string ?? "uk";
            properties.Items["culture"] = culture;
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            // Handle Telegram login
            if (string.Equals(Provider, "Telegram", StringComparison.OrdinalIgnoreCase))
            {
                var botToken = _config["InvestRadar:Authentication:Telegram:BotToken"];
                if (!VerifyTelegramLogin(Request.Query, botToken))
                {
                    _logger.LogError("Invalid Telegram login signature.");
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }

                var loginInfo = new UserLoginInfo("Telegram", Id, "Telegram");
                var signInResult = await _signInManager.ExternalLoginSignInAsync(
                    loginInfo.LoginProvider,
                    loginInfo.ProviderKey,
                    isPersistent: false,
                    bypassTwoFactor: true);

                if (signInResult.Succeeded)
                {
                    _logger.LogInformation("User logged in with Telegram.");
                    return LocalRedirect(returnUrl);
                }
                if (signInResult.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }

                // New user flow: create and link
                var user = new User
                {
                    UserName = !string.IsNullOrEmpty(Username) ? Username : $"tg_{Id}",
                    Email = Input?.Email ?? $"{Id}@telegram.local",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    foreach (var e in createResult.Errors)
                        ModelState.AddModelError(string.Empty, e.Description);
                    return Page();
                }

                await _userManager.AddLoginAsync(user, loginInfo);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            if (remoteError != null)
            {
                _logger.LogError("Error from external provider: {RemoteError}", remoteError);
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError("Error loading external login information.");
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var cultureVal = info.AuthenticationProperties.Items.ContainsKey("culture")
                ? info.AuthenticationProperties.Items["culture"]
                : "uk";
            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider}.",
                    info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect($"/{cultureVal}{returnUrl}");
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    var createRes = await _userManager.CreateAsync(user);
                    if (createRes.Succeeded)
                    {
                        await _userManager.AddLoginAsync(user, info);
                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                    return LocalRedirect(returnUrl);
                }
            }

            // Fallback: prompt for email
            ReturnUrl = returnUrl;
            ProviderDisplayName = info.ProviderDisplayName;
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                Input = new InputModel
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };
            }
            return Page();
        }

        private bool VerifyTelegramLogin(IQueryCollection query, string botToken)
        {
            if (!query.TryGetValue("hash", out var hashValues))
                return false;
            var providedHash = hashValues.ToString();

            var allowed = new[] { "auth_date", "first_name", "id", "last_name", "photo_url", "username" };
            var dataCheck = query
                .Where(kv => allowed.Contains(kv.Key))
                .Select(kv => $"{kv.Key}={kv.Value}")
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToArray();
            var dataString = string.Join("\n", dataCheck);

            byte[] secret;
            using (var sha = SHA256.Create())
            {
                secret = sha.ComputeHash(Encoding.UTF8.GetBytes(botToken));
            }
            byte[] hash;
            using (var hmac = new HMACSHA256(secret))
            {
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataString));
            }
            var computed = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return computed == providedHash;
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
                throw new NotSupportedException("User store needs email support.");
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
