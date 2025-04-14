// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Core.Entities;
using Core.Interfaces;
using InvestList.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Radar.Domain.Entities;

namespace InvestList.Areas.Identity.Pages.Account
{
    public class RegisterModel(
        UserManager<User> userManager,
        IUserStore<User> userStore,
        SignInManager<User> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender,
        IUserRepository userRepository)
        : PageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            
            public int TimeSpent { get; set; }
            public bool MouseMoved { get; set; }
            public bool NavigatorWebdriver { get; set; }
            public bool HasChrome { get; set; }
            public int ScreenHeight { get; set; }
            public int ScreenWidth { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(Input.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Емейл вже використовується. Спробуйте інший, або відновіть доступ.");
                    return Page();
                }
                
                var user = CreateUser();

                await userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                var result = await userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");
                    var roleAssign = await userManager.AddToRoleAsync(user, Const.BusinessRole);
                    if (roleAssign.Succeeded)
                    {
                        logger.LogInformation("Role assigned for the user");
                    }
                    else
                    {
                        logger.LogError("Role wasn't assigned {@Error}", roleAssign.Errors);
                    }
                    
                    var userId = await userManager.GetUserIdAsync(user);
                    var requestInfo = InvestmentHelper.GetRequestInfo(HttpContext, Input.Username, new UserDetectionInfo()
                    {
                        HasChrome = Input.HasChrome,
                        MouseMoved = Input.MouseMoved,
                        NavigatorWebdriver = Input.NavigatorWebdriver,
                        ScreenWidth = Input.ScreenWidth,
                        ScreenHeight = Input.ScreenHeight,
                        TimeSpent = Input.TimeSpent,
                        
                    });
                    await userRepository.SaveRequestInfo(requestInfo);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    try
                    {

                        await emailSender.SendEmailAsync(Input.Email, "Підтвердіть свою реєстрацію на invest-radar.com",
                            """
                            <div>Привіт,</div>
                            <div></div>
                            <div>Дякуємо за реєстрацію на invest-radar.com</div>
                            <div>Щоб завершити процес, будь ласка, підтвердіть свою електронну адресу </div>
                            <div>Якщо ви не реєструвалися на нашому сайті, просто ігноруйте цей лист.</div>
                            <div></div>
                            <div>З повагою, </div>
                            <div>Команда invest-radar.com</div>
                            <div></div>
                            """ +
                            $"<div>Будь ласка, підтвердіть акаунт, <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>перейшовши за посиланням</a>.</div>");
                    }
                    catch (Exception ex)
                    {
                        await userRepository.MarkAsFailedToSendEmail(user.Id);
                    }
                    
                    if (userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        
        private User CreateUser()
        {
            try
            {
                var instance = Activator.CreateInstance<User>();
                instance.Email = Input.Email;
                return instance;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
