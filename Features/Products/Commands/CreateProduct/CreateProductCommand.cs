using MediatR;
using SynchApp.Dtos;
using SynchApp.Models;

namespace SynchApp.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Product>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    
    // Factory method to create from DTO
    public static CreateProductCommand FromDto(ProductCreateDto dto)
    {
        return new CreateProductCommand
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            
        };
    }
}