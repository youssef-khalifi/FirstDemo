using MediatR;
using SynchApp.Models;

namespace SynchApp.Features.Collections.Queries.GetAllCollections;

public class GetAllCollectionsQuery : IRequest<IEnumerable<Collection>>
{
    
}