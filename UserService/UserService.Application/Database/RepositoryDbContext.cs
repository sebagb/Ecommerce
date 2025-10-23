namespace UserService.Application.Database;

using Microsoft.EntityFrameworkCore;
using UserService.Application.Models;

public class RepositoryDbContext
    (DbContextOptions options) : DbContext(options)
{
    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(x => new { x.Username })
            .IsUnique();
    }
}