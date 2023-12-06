
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

public class LoginController: Controller
{
    private readonly SignInManager<User> _signInManager;

    public LoginController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home"); // Redirect to home page on successful login
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
        }

        return View(model);
    }
}
