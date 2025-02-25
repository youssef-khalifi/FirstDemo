using MediatR;
using SynchApp.Dtos;
using SynchApp.Models;

namespace SynchApp.Features.Collections.Commands.CreateCollection;

public class CreateCollectionCommand : IRequest<Collection>
{
    public string Name { get; set; } = string.Empty;
    
    public static CreateCollectionCommand FromDto(CreateCollectionDto dto)
    {
        return new CreateCollectionCommand
        {
            Name = dto.Name,
        };
    }
}