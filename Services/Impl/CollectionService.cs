using Microsoft.EntityFrameworkCore;
using SynchApp.Data;
using SynchApp.Dtos;
using SynchApp.Models;
using SynchApp.Services.Interfaces;

namespace SynchApp.Services;

public class CollectionService : ICollectionService
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<ProductService> _logger;
    private readonly ISyncService _syncService;

    public CollectionService(SqliteDbContext sqliteContext, ILogger<ProductService> logger, ISyncService syncService)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
        _syncService = syncService;
    }

    public async Task<IEnumerable<Collection>> GetAllCollectionsAsync()
    {
        try
        {
            return await _sqliteContext.Collections.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all collections");
            throw;
        }
    }

    public async Task<Collection> GetCollectionByIdAsync(int id)
    {
        try
        {
            var product = await _sqliteContext.Collections.FindAsync(id);
                
            if (product == null)
            {
                _logger.LogWarning($"Collection with ID {id} was not found");
                return null;
            }
                
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving collection with ID {id}");
            throw;
        }
    }

    public async Task<Collection> CreateCollectionAsync(CreateCollectionDto collectionDto)
    {
        try
        {
            // Map DTO to entity
            var collection = new Collection
            {
                Name = collectionDto.Name,
                CreatedAt = DateTime.UtcNow,
                IsSynced = false
            };
                
            await _sqliteContext.Collections.AddAsync(collection);
            await _sqliteContext.SaveChangesAsync();
                
            _logger.LogInformation($"Created new collection with ID {collection.Id}");
                
            return collection;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new collection");
            throw;
        }
    }

    public Task<Collection> AddRequestToCollection(CreateRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteCollectionAsync(int id)
    {
        try
        {
            // First check if product exists
            var product = await _sqliteContext.Collections.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning($"Collection with ID {id} not found for deletion");
                return false;
            }
        }catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting collection with ID {id}");
            throw;
        }


        return true;
    }
}