using PryanikyTest.Application.Features.Products.Commands;
using PryanikyTest.Application.Features.Products.Queries;
using PryanikyTest.Application.Features.Products.Dto;
using PryanikyTest.Application.Pagination;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace PryanikyTest.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<ProductLookupDto>>> GetAllProducts(
        string? search, Guid? sellerId, string? sortColumn, string? sortOrder,
        int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetAllProductsQuery(
            search, sellerId, sortColumn, sortOrder, new Page(page, pageSize));

        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ProductCreatedDto>> CreateProduct(
        [FromForm] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(CreateProduct), response);
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<ProductDto>> GetProductById(
        Guid productId, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(productId);
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{productId}")]
    public async Task<ActionResult> DeleteProduct(
        Guid productId, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(productId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
