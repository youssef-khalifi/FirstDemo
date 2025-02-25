using SynchApp.Models;
using SynchApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using SynchApp.Features.Products.Queries.SynchProducts;

namespace SyncApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SynchProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        

        public SynchProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Products/sync
        [HttpGet("sync")]
        public async Task<IActionResult> SyncProducts([FromQuery] bool syncNow = false)
        {
            var result = await _mediator.Send(new SyncProductsQuery(syncNow));

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }

       
        
}