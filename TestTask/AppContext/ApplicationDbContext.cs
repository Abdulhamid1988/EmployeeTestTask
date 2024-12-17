using Microsoft.EntityFrameworkCore;
using TestTask.Entities;

namespace TestTask.AppContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }
    public DbSet<Employee> Employees { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Employee>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
    }
}