using MediatR;
using SynchApp.Models;
using SynchApp.Services;

namespace SynchApp.Features.Products.Queries.SynchProducts;

public class SyncProductsQueryHandler : IRequestHandler<SyncProductsQuery, SyncResult>
{
    private readonly ISyncService _syncService;
    private readonly ILogger<SyncProductsQueryHandler> _logger;


    public SyncProductsQueryHandler(ISyncService syncService, ILogger<SyncProductsQueryHandler> logger)
    {
        _syncService = syncService;
        _logger = logger;
    }

    public async Task<SyncResult> Handle(SyncProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.SyncNow)
            {
                return new SyncResult
                {
                    Success = true,
                    Message = "Sync not triggered. Pass syncNow=true to trigger synchronization."
                };
            }

            return await _syncService.SyncProductsToSqlServer();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during synchronization");
            return new SyncResult
            {
                Success = false,
                Message = $"Sync failed: {ex.Message}"
            };
        }
    }
}