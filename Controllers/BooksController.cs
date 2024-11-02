using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    /* public IActionResult Index(int page = 1)
     {
         Console.WriteLine($"Запрос на страницу Index, страница: {page}");

         int pageSize = 6;


         var books = _context.Books
             .FromSqlRaw(@"
             SELECT id, author, coverphoto, dateadded, description, title, year, ""UserId"", isborrowed
             FROM ""books""
             ORDER BY dateadded DESC
         ")
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .ToList();


         var totalBooks = _context.Books.Count();
         Console.WriteLine($"Общее количество книг: {totalBooks}");
         Console.WriteLine($"Количество книг на странице: {books.Count}");

         var model = new BookViewModel
         {
             Books = books,
             TotalBooks = totalBooks,
             PageSize = pageSize,
             CurrentPage = page
         };

         return View(model);
     }*/

    public IActionResult Index(int page = 1, string title = "", string author = "", bool? isBorrowed = null)
    {
        Console.WriteLine($"Запрос на страницу Index, страница: {page}");

        int pageSize = 6;
        var query = _context.Books.AsQueryable();

        // Применение фильтров
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(b => b.title.Contains(title));
        }

        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(b => b.author.Contains(author));
        }

        if (isBorrowed.HasValue)
        {
            query = query.Where(b => b.isborrowed == isBorrowed.Value);
        }

        var totalBooks = query.Count();
        Console.WriteLine($"Общее количество книг: {totalBooks}");

        // Выполнение запроса
        var books = query
            .OrderByDescending(b => b.dateadded)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        Console.WriteLine($"Количество книг на странице: {books.Count}");

        var model = new BookViewModel
        {
            Books = books,
            TotalBooks = totalBooks,
            PageSize = pageSize,
            CurrentPage = page,
            Filter = new BookFilterModel
            {
                Title = title,
                Author = author,
                IsBorrowed = isBorrowed
            }
        };

        return View(model);
    }




    public IActionResult Details(int id)
    {
        var book = _context.Books
            .FromSqlRaw(@"
            SELECT id, author, coverphoto, dateadded, description, title, year, ""UserId"", isborrowed
            FROM ""books""
            WHERE id = @id", new NpgsqlParameter("id", id))
            .FirstOrDefault();

        if (book == null)
        {
            return NotFound();
        }

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
    public IActionResult BorrowBook(int bookId, string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            Console.WriteLine($"Пользователь с email {email} не найден.");
            ModelState.AddModelError("", "Пользователь с таким email не найден.");
            return RedirectToAction("Index");
        }

        var borrowedBooksCount = _context.Books.Count(b => b.UserId == user.Id && b.isborrowed);
        if (borrowedBooksCount >= 3)
        {
            Console.WriteLine($"Пользователь {user.Email} уже взял максимальное количество книг.");
            ModelState.AddModelError("", "Вы не можете взять более 3 книг.");
            return RedirectToAction("Index");
        }

        var book = _context.Books.Find(bookId);
        if (book == null)
        {
            Console.WriteLine($"Книга с id {bookId} не найдена.");
            return NotFound();
        }

        if (book.isborrowed)
        {
            Console.WriteLine($"Книга {book.title} уже взята.");
            ModelState.AddModelError("", "Эта книга уже взята.");
            return RedirectToAction("Index");
        }

        book.isborrowed = true;
        book.UserId = user.Id; 
        _context.SaveChanges();

        Console.WriteLine($"Книга {book.title} успешно выдана пользователю {user.Email}.");
        return RedirectToAction("Index");
    }




    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.id == id);
    }
}
