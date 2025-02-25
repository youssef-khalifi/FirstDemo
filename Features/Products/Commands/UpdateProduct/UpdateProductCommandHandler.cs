using MediatR;
using SynchApp.Data;
using SynchApp.Models;

namespace SynchApp.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
{
    private readonly SqliteDbContext _sqliteContext;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(SqliteDbContext sqliteContext, ILogger<UpdateProductCommandHandler> logger)
    {
        _sqliteContext = sqliteContext;
        _logger = logger;
    }

    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _sqliteContext.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning($"Product with ID {request.Id} not found for update");
                return null;
            }

            // Update product properties
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.UpdatedAt = DateTime.UtcNow;
            product.IsSynced = false;

            _sqliteContext.Products.Update(product);
            await _sqliteContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Updated product with ID {request.Id}");

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating product with ID {request.Id}");
            throw;
        }
    }
}