using MediatR;
using SynchApp.Models;

namespace SynchApp.Features.Products.Queries.SynchProducts;

public class SyncProductsQuery : IRequest<SyncResult>
{
    public bool SyncNow { get; set; }

    public SyncProductsQuery(bool syncNow = false)
    {
        SyncNow = syncNow;
    }
}