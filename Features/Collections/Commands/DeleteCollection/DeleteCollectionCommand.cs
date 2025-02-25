using MediatR;

namespace SynchApp.Features.Collections.Commands.DeleteCollection;

public class DeleteCollectionCommand : IRequest<bool>
{
    public int Id { get; set; }
    
    public DeleteCollectionCommand(int id)
    {
        Id = id;
    }
}