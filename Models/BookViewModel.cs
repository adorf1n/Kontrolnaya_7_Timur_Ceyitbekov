public class BookViewModel
{
    public List<Book> Books { get; set; }
    public int TotalBooks { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public BookFilterModel Filter { get; set; } 
}
