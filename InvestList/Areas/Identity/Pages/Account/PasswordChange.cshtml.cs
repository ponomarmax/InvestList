using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Core.Entities; 

namespace InvestList.Areas.Identity.Pages.Account
{
    public class PasswordChangeModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PasswordChangeModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required(ErrorMessage = "Поточний пароль є обов'язковим")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Новий пароль є обов'язковим")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Підтвердження паролю є обов'язковим")]
        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не вдалося завантажити користувача з ID '{_userManager.GetUserId(User)}'.");
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, CurrentPassword);
            if (!passwordCheck)
            {
                ModelState.AddModelError(string.Empty, "Пароль невірний.");
                ErrorMessage = "Пароль невірний";
                return Page();
            }

            var passwordRequirements = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{5,}$");
            if (!passwordRequirements.IsMatch(NewPassword))
            {
                ModelState.AddModelError(string.Empty, "Новий пароль не відповідає вимогам: має містити щонайменше одну велику літеру, одну маленьку літеру, одну цифру та один спеціальний символ.");
                ErrorMessage = "Пароль має відповідати таким вимогам:" +
                                "- Пароль має містити хоча б 1 велику літеру\n" +
                                "- Пароль має містити хоча б 1 маленьку літеру\n" +
                                "- Пароль має містити хоча б 1 цифру\n" +
                                "- Пароль має містити хоча б 1 спеціальний символ\n" +
                                "- Всі літери мають бути написані латиницею";
                return Page();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            SuccessMessage = "Пароль успішно змінено";
            return RedirectToPage();
        }
    }
}
