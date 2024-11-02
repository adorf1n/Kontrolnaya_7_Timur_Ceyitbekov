using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;

    public UsersController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        Console.WriteLine($"Попытка создания пользователя: {user.FirstName} {user.LastName}, Email: {user.Email}");

        if (ModelState.IsValid)
        {
            user.UserName = user.Email; 

            var result = await _userManager.CreateAsync(user, "Password123!"); 

            if (result.Succeeded)
            {
                Console.WriteLine("Пользователь успешно создан.");
                return RedirectToAction("Index", "Books");
            }

            Console.WriteLine("Ошибка создания пользователя:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Ошибка: {error.Description}");
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        else
        {
            Console.WriteLine("Модель недействительна:");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Ошибка: {error.ErrorMessage}");
                }
            }
        }

        return View(user);
    }

}