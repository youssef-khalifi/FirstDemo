using MediatR;
using SynchApp.Data;
using SynchApp.Services;

namespace SynchApp.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    
    private readonly SqliteDbContext _sqliteContext;
    private readonly ISyncService _syncService;
    private readonly ILogger<DeleteProductCommandHandler> _logger;


    public DeleteProductCommandHandler(SqliteDbContext sqliteContext, ISyncService syncService, ILogger<DeleteProductCommandHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _syncService = syncService;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _sqliteContext.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning($"Product with ID {request.Id} not found for deletion");
                return false;
            }

            var result = await _syncService.DeleteProduct(request.Id);

            if (result)
            {
                _logger.LogInformation($"Deleted product with ID {request.Id}");
            }
            else
            {
                _logger.LogWarning($"Failed to delete product with ID {request.Id}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting product with ID {request.Id}");
            throw;
        }
    }
}