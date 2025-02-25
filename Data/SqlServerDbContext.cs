using Microsoft.EntityFrameworkCore;
using SynchApp.Models;

namespace SynchApp.Data;

public class SqlServerDbContext : DbContext
{
    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft delete query filter
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
    }
}

