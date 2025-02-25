using MediatR;
using SynchApp.Models;

namespace SynchApp.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
{
    
}