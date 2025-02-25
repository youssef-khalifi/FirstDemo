using Microsoft.EntityFrameworkCore;
using SynchApp.Data;
using SynchApp.Dtos;
using SynchApp.Models;

namespace SynchApp.Services;

public class ProductService : IProductService
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<ProductService> _logger;
    private readonly ISyncService _syncService;


    public ProductService(SqliteDbContext sqliteContext, ILogger<ProductService> logger, ISyncService syncService)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
        _syncService = syncService;
    }

    
    // Create new product
    public async Task<Product> CreateProductAsync(ProductCreateDto productDto)
    {
        try
        {
            // Map DTO to entity
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CreatedAt = DateTime.UtcNow,
                IsSynced = false
            };
                
            await _sqliteContext.Products.AddAsync(product);
            await _sqliteContext.SaveChangesAsync();
                
            _logger.LogInformation($"Created new product with ID {product.Id}");
                
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new product");
            throw;
        }
    }
    
    // Get all products
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            return await _sqliteContext.Products.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            throw;
        }
    }


    // Get product by ID
    public async Task<Product> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _sqliteContext.Products.FindAsync(id);
                
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} was not found");
                return null;
            }
                
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving product with ID {id}");
            throw;
        }
    }
    
    // Update existing product
    public async Task<Product> UpdateProductAsync(Product product)
    {
        try
        {
            var existingProduct = await _sqliteContext.Products.FindAsync(product.Id);
                
            if (existingProduct == null)
            {
                _logger.LogWarning($"Product with ID {product.Id} not found for update");
                return null;
            }
                
            // Update properties
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            existingProduct.IsSynced = false;
                
            _sqliteContext.Products.Update(existingProduct);
            await _sqliteContext.SaveChangesAsync();
                
            _logger.LogInformation($"Updated product with ID {product.Id}");
                
            return existingProduct;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating product with ID {product.Id}");
            throw;
        }
    }

    // Delete product (uses the sync service to ensure deletion in both databases)
    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            // First check if product exists
            var product = await _sqliteContext.Products.FindAsync(id);
                
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found for deletion");
                return false;
            }
                
            // Use the sync service to handle deletion in both databases
            var result = await _syncService.DeleteProduct(id);
                
            if (result)
            {
                _logger.LogInformation($"Deleted product with ID {id}");
            }
            else
            {
                _logger.LogWarning($"Failed to delete product with ID {id}");
            }
                
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID {id}");
            throw;
        }
    }
}