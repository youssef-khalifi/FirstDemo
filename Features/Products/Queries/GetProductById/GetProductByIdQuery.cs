using MediatR;
using SynchApp.Models;

namespace SynchApp.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<Product>
{
    public int Id { get; set; }
    
    public GetProductByIdQuery(int id)
    {
        Id = id;
    }
}