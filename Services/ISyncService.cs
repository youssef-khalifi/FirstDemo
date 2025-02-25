using SynchApp.Models;

namespace SynchApp.Services;

public interface ISyncService
{
    Task<SyncResult> SyncProductsToSqlServer();
    Task<bool> SaveProductToSqlite(Product product);
    Task<bool> DeleteProduct(int productId);
}