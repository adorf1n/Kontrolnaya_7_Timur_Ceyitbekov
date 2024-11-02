using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    public bool CanConnect()
    {
        try
        {
            return this.Database.CanConnect();
        }
        catch (Exception)
        {
            return false;
        }
    }
}