using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public UsersController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        Console.WriteLine("Запрос списка пользователей и количества их книг.");

        var users = await _userManager.Users
            .Select(user => new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                BorrowedBooksCount = _context.Books.Count(b => b.UserId == user.Id && b.isborrowed)
            })
            .ToListAsync();

        Console.WriteLine($"Найдено пользователей: {users.Count}");

        return View(users);
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
