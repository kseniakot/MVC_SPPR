using Microsoft.EntityFrameworkCore;
using WEB_253503_KOTOVA.Domain;
using WEB_253503_KOTOVA.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Dish> Dishes { get; set; }
}
