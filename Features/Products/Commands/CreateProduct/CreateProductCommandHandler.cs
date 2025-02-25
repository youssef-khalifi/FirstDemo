using MediatR;
using SynchApp.Data;
using SynchApp.Models;

namespace SynchApp.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(SqliteDbContext sqliteContext, ILogger<CreateProductCommandHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CreatedAt = DateTime.UtcNow,
                IsSynced = false
            };

            await _sqliteContext.Products.AddAsync(product, cancellationToken);
            await _sqliteContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Created new product with ID {product.Id}");

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new product");
            throw;
        }
    }

}