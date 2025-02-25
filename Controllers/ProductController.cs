using MediatR;
using Microsoft.AspNetCore.Mvc;
using SynchApp.Dtos;
using SynchApp.Features.Products.Commands.CreateProduct;
using SynchApp.Features.Products.Commands.DeleteProduct;
using SynchApp.Features.Products.Commands.UpdateProduct;
using SynchApp.Features.Products.Queries.GetAllProducts;
using SynchApp.Features.Products.Queries.GetProductById;
using SynchApp.Models;
using SynchApp.Services;

namespace SyncApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCreateDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = CreateProductCommand.FromDto(productDto);
        var createdProduct = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = createdProduct.Id },
            createdProduct);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }
    
    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
    
    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = UpdateProductCommand.FromDto(id, productDto);
        var updatedProduct = await _mediator.Send(command);

        if (updatedProduct == null)
        {
            return NotFound();
        }

        return Ok(updatedProduct);
    }
    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));

        if (!result)
        {
            return NotFound();
        }

        return Ok(new { Message = $"Product with ID {id} has been deleted" });
    }
}