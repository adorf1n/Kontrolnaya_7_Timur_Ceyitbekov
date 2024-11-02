using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(int page = 1)
    {
        Console.WriteLine($"Запрос на страницу Index, страница: {page}");

        int pageSize = 6; 

        var books = _context.Books
            .FromSqlRaw(@"
            SELECT id, author, coverphoto, dateadded, description, title, year
            FROM ""books""
            ORDER BY dateadded DESC
        ")
            .ToList();

        int totalBooks = books.Count; 

        Console.WriteLine($"Общее количество книг: {totalBooks}"); 

        var pagedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        Console.WriteLine($"Количество книг на странице: {pagedBooks.Count}");

        var model = new BookViewModel
        {
            Books = pagedBooks,
            TotalBooks = totalBooks,
            PageSize = pageSize,
            CurrentPage = page
        };

        return View(model);
    }

    public IActionResult Details(int id)
    {
        Console.WriteLine($"Запрос на страницу деталей книги с id: {id}");

        var book = _context.Books
            .FromSqlRaw("SELECT id, author, coverphoto, dateadded, description, title, year FROM \"books\" WHERE id = {0} LIMIT 1", id)
            .AsEnumerable()
            .FirstOrDefault();

        if (book == null)
        {
            Console.WriteLine($"Книга с id {id} не найдена.");
            return NotFound();
        }

        Console.WriteLine($"Книга найдена: {book.title}, автор: {book.author}");
        return View(book);
    }



    public IActionResult Create()
    {
        Console.WriteLine("Переход на страницу создания книги.");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        Console.WriteLine($"Попытка создания книги: {book.title} от автора {book.author}");

        if (ModelState.IsValid)
        {
            book.dateadded = DateTime.Now;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            Console.WriteLine("Книга успешно добавлена.");
            return RedirectToAction(nameof(Index));
        }

        Console.WriteLine("Ошибка в модели при создании книги.");
        return View(book);
    }

    public IActionResult Edit(int id)
    {
        Console.WriteLine($"Переход на страницу редактирования книги с id: {id}");
        var book = _context.Books.Find(id);

        if (book == null)
        {
            Console.WriteLine($"Книга с id {id} не найдена для редактирования.");
            return NotFound();
        }

        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book)
    {
        if (id != book.id)
        {
            Console.WriteLine($"Id из запроса {id} не соответствует id книги {book.id}.");
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
                Console.WriteLine("Книга успешно обновлена.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.id))
                {
                    Console.WriteLine($"Книга с id {book.id} не существует.");
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        Console.WriteLine("Ошибка в модели при редактировании книги.");
        return View(book);
    }

    [HttpPost]
    public IActionResult Borrow(int id)
    {
        // Здесь вы можете добавить логику для получения книги,
        // например, обновление статуса книги или записи о том, кто её взял.

        var book = _context.Books.Find(id);
        if (book == null)
        {
            Console.WriteLine($"Книга с id {id} не найдена.");
            return NotFound();
        }

        // Пример логики, когда книга была взята:
        // book.Status = "Выдана"; // если у вас есть поле для статуса
        // _context.SaveChanges(); // сохраните изменения в базе данных

        Console.WriteLine($"Книга {book.title} была успешно получена.");
        return RedirectToAction("Index"); // Перенаправляем обратно на главную страницу
    }


    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.id == id);
    }
}
