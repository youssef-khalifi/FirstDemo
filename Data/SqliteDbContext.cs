using Microsoft.EntityFrameworkCore;
using SynchApp.Models;

namespace SynchApp.Data;

public class SqliteDbContext : DbContext
{
    public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<RequestApi> Requests { get; set; }
    public DbSet<ResponseApi> Responses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft delete query filter
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Collection>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<RequestApi>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<ResponseApi>().HasQueryFilter(r => !r.IsDeleted);
    }
    
}