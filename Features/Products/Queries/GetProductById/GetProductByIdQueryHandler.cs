using MediatR;
using SynchApp.Data;
using SynchApp.Models;

namespace SynchApp.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;


    public GetProductByIdQueryHandler(SqliteDbContext sqliteContext, ILogger<GetProductByIdQueryHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _sqliteContext.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning($"Product with ID {request.Id} was not found");
            }

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving product with ID {request.Id}");
            throw;
        }
    }
}