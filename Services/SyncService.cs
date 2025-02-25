
using Microsoft.EntityFrameworkCore;
using SynchApp.Data;
using SynchApp.Models;

namespace SynchApp.Services;

public class SyncService : ISyncService
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly SqlServerDbContext _sqlServerContext;
    private readonly ILogger<SyncService> _logger;


    public SyncService(SqliteDbContext sqliteContext, SqlServerDbContext sqlServerContext, ILogger<SyncService> logger)
    {
        _sqliteContext = sqliteContext;
        _sqlServerContext = sqlServerContext;
        _logger = logger;
    }

    public async Task<SyncResult> SyncProductsToSqlServer()
    {
        var result = new SyncResult { Success = true };

            try
            {
                // Get all products from SQLite that need syncing (including deleted ones)
                var sqliteProducts = await _sqliteContext.Products
                    .IgnoreQueryFilters() // Include soft-deleted items
                    .Where(p => !p.IsSynced)
                    .ToListAsync();

                foreach (var sqliteProduct in sqliteProducts)
                {
                    // Check if product exists in SQL Server
                    var sqlServerProduct = await _sqlServerContext.Products
                        .IgnoreQueryFilters() // Include soft-deleted items
                        .FirstOrDefaultAsync(p => p.Id == sqliteProduct.Id);

                    if (sqlServerProduct == null)
                    {
                        // Product doesn't exist in SQL Server, add it
                        if (!sqliteProduct.IsDeleted)
                        {
                            var newProduct = new Product
                            {
                                
                                Name = sqliteProduct.Name,
                                Description = sqliteProduct.Description,
                                Price = sqliteProduct.Price,
                                CreatedAt = sqliteProduct.CreatedAt,
                                UpdatedAt = sqliteProduct.UpdatedAt,
                                IsDeleted = sqliteProduct.IsDeleted
                            };

                            await _sqlServerContext.Products.AddAsync(newProduct);
                            result.Added++;
                        }
                    }
                    else
                    {
                        // Product exists in SQL Server, update it
                        if (sqliteProduct.IsDeleted)
                        {
                            // Mark as deleted in SQL Server if deleted in SQLite
                            sqlServerProduct.IsDeleted = true;
                            sqlServerProduct.UpdatedAt = DateTime.UtcNow;
                            result.Deleted++;
                        }
                        else
                        {
                            // Update existing record
                            sqlServerProduct.Name = sqliteProduct.Name;
                            sqlServerProduct.Description = sqliteProduct.Description;
                            sqlServerProduct.Price = sqliteProduct.Price;
                            sqlServerProduct.UpdatedAt = DateTime.UtcNow;
                            result.Updated++;
                        }

                        _sqlServerContext.Products.Update(sqlServerProduct);
                    }

                    // Mark as synced in SQLite
                    sqliteProduct.IsSynced = true;
                    sqliteProduct.LastSyncedAt = DateTime.UtcNow;
                    _sqliteContext.Products.Update(sqliteProduct);
                }

                // Save changes to both databases
               // await _sqlServerContext.SaveChangesAsync();
              //  await _sqliteContext.SaveChangesAsync();
              
              var sqlServerTransaction = await _sqlServerContext.Database.BeginTransactionAsync();
              var sqliteTransaction = await _sqliteContext.Database.BeginTransactionAsync();

              try
              {
                  await _sqlServerContext.SaveChangesAsync();
                  await _sqliteContext.SaveChangesAsync();
    
                  await sqlServerTransaction.CommitAsync();
                  await sqliteTransaction.CommitAsync();
              }
              catch
              {
                  await sqlServerTransaction.RollbackAsync();
                  await sqliteTransaction.RollbackAsync();
                  throw;
              }

              
                result.Message = $"Sync completed successfully. Added: {result.Added}, Updated: {result.Updated}, Deleted: {result.Deleted}";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Sync failed: {ex.Message}";
                _logger.LogError(ex, "Error during database synchronization");
            }

            return result;
    }

    public async Task<bool> SaveProductToSqlite(Product product)
    {
        try
        {
            if (product.Id == 0)
            {
                // New product
                product.CreatedAt = DateTime.UtcNow;
                product.IsSynced = false;
                await _sqliteContext.Products.AddAsync(product);
            }
            else
            {
                // Existing product
                var existingProduct = await _sqliteContext.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == product.Id);

                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {product.Id} not found in SQLite.");
                    return false;
                }

                product.UpdatedAt = DateTime.UtcNow;
                product.IsSynced = false;
                _sqliteContext.Entry(product).State = EntityState.Modified;
            }

            await _sqliteContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving product to SQLite");
            return false;
        }
    }

    public async Task<bool> DeleteProduct(int productId)
    {
        try
        {
            var sqliteTask = Task.Run(async () =>
            {
                var sqliteProduct = await _sqliteContext.Products
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (sqliteProduct != null)
                {
                    sqliteProduct.IsDeleted = true;
                    sqliteProduct.UpdatedAt = DateTime.UtcNow;
                    sqliteProduct.IsSynced = false;
                    await _sqliteContext.SaveChangesAsync();
                }
            });

            var sqlServerTask = Task.Run(async () =>
            {
                var sqlServerProduct = await _sqlServerContext.Products
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (sqlServerProduct != null)
                {
                    sqlServerProduct.IsDeleted = true;
                    sqlServerProduct.UpdatedAt = DateTime.UtcNow;
                    await _sqlServerContext.SaveChangesAsync();
                }
            });

            await Task.WhenAll(sqliteTask, sqlServerTask);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID {productId}");
            return false;
        }
    }

}