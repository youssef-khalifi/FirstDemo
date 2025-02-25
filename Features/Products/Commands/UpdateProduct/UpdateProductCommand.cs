using MediatR;
using SynchApp.Dtos;
using SynchApp.Models;

namespace SynchApp.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Product>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    
    
    public static UpdateProductCommand FromDto(int id, ProductUpdateDto dto)
    {
        return new UpdateProductCommand
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
        };
    }
}