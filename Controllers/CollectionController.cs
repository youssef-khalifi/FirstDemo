using MediatR;
using Microsoft.AspNetCore.Mvc;
using SynchApp.Dtos;
using SynchApp.Features.Collections.Commands.CreateCollection;
using SynchApp.Features.Collections.Queries.GetAllCollections;
using SynchApp.Models;

namespace SynchApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollectionController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCollections()
    {
        var collections = await _mediator.Send(new GetAllCollectionsQuery());
        return Ok(collections);
    }
    [HttpPost]
    public async Task<ActionResult<Collection>> CreateProduct([FromBody] CreateCollectionDto collectionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = CreateCollectionCommand.FromDto(collectionDto);
        var createdCollection = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetCollection),
            new { id = createdCollection.Id },
            createdCollection);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetCollection(int id)
    {
        return Ok();
    }

}