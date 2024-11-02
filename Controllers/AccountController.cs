using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        Console.WriteLine("Попытка входа пользователя.");

        if (ModelState.IsValid)
        {
            Console.WriteLine($"Email: {model.Email}");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                Console.WriteLine("Авторизация успешна.");
                return RedirectToAction("Index", "Books");
            }
            else
            {
                Console.WriteLine("Ошибка авторизации: неверный логин или пароль.");
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
            }
        }
        else
        {
            Console.WriteLine("Модель невалидна:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Ошибка: {error.ErrorMessage}");
            }
        }

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Books");
    }
}
