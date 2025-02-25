using MediatR;
using Microsoft.EntityFrameworkCore;
using SynchApp.Data;
using SynchApp.Models;

namespace SynchApp.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;

    public GetAllProductsQueryHandler(SqliteDbContext sqliteContext, ILogger<GetAllProductsQueryHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _sqliteContext.Products.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            throw;
        }
    }

}