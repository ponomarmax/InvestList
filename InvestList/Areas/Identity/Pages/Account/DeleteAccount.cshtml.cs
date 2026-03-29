using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Radar.Domain.Entities;

namespace InvestList.Areas.Identity.Pages.Account
{
    public class DeleteAccountModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public DeleteAccountModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

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

            var passwordCheck = await _userManager.CheckPasswordAsync(user, Password);
            if (!passwordCheck)
            {
                ModelState.AddModelError(string.Empty, "Пароль невірний.");
                ErrorMessage = "Пароль невірний";
                return Page();
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                foreach (var error in deleteResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
