using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .ToTable("books")
            .HasOne(b => b.User)
            .WithMany(u => u.Books)
            .HasForeignKey(b => b.UserId); 
    }

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