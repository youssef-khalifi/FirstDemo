using MediatR;
using SynchApp.Data;
using SynchApp.Features.Products.Commands.DeleteProduct;
using SynchApp.Services;

namespace SynchApp.Features.Collections.Commands.DeleteCollection;

public class DeleteCollectionCommandHandler : IRequestHandler<DeleteCollectionCommand, bool>
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly ISyncService _syncService;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteCollectionCommandHandler(SqliteDbContext sqliteContext, ISyncService syncService, ILogger<DeleteProductCommandHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _syncService = syncService;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteCollectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _sqliteContext.Collections.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning($"Collection with ID {request.Id} not found for deletion");
                return false;
            }
            //await _syncService.DeleteCollectionAsync(product); handle sync in sqlServer
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID {request.Id}");
            throw;
        }
    }
}