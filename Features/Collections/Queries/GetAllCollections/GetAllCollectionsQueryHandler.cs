using MediatR;
using Microsoft.EntityFrameworkCore;
using SynchApp.Data;
using SynchApp.Models;

namespace SynchApp.Features.Collections.Queries.GetAllCollections;

public class GetAllCollectionsQueryHandler : IRequestHandler<GetAllCollectionsQuery , IEnumerable<Collection>>
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<GetAllCollectionsQuery> _logger;

    public GetAllCollectionsQueryHandler(SqliteDbContext sqliteContext, ILogger<GetAllCollectionsQuery> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Collection>> Handle(GetAllCollectionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _sqliteContext.Collections.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all collections");
            throw;
        }
    }
}