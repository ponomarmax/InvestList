using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace InvestList.Areas.Identity.Pages.Account
{
    public class LoginChangeModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginChangeModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required]
        public string NewUsername { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        [TempData]
        public string MessageStatus { get; set; }

        public async Task<IActionResult> OnPostChangeUsernameAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, Password, lockoutOnFailure: false);
            if (!passwordCheck.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Неправильний пароль");
                return Page();
            }

            var setUserNameResult = await _userManager.SetUserNameAsync(user, NewUsername);
            if (!setUserNameResult.Succeeded)
            {
                foreach (var error in setUserNameResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            MessageStatus = "Логін успішно змінено";
            return RedirectToPage();
        }
    }
}