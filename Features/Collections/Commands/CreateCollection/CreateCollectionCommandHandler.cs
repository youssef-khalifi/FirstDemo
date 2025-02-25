using MediatR;
using SynchApp.Data;
using SynchApp.Models;

namespace SynchApp.Features.Collections.Commands.CreateCollection;

public class CreateCollectionCommandHandler : IRequestHandler<CreateCollectionCommand, Collection>
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<CreateCollectionCommandHandler> _logger;

    public CreateCollectionCommandHandler(SqliteDbContext sqliteContext, ILogger<CreateCollectionCommandHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<Collection> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var collection = new Collection
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                IsSynced = false
            };

            await _sqliteContext.Collections.AddAsync(collection, cancellationToken);
            await _sqliteContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Created new collection with ID {collection.Id}");

            return collection;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new collection");
            throw;
        }
    }
}