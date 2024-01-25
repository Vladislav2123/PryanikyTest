using PryanikyTest.Application.Features.Sellers.Commands;
using PryanikyTest.Application.Features.Sellers.Queries;
using PryanikyTest.Application.Features.Sellers.Dto;
using PryanikyTest.Application.Pagination;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace PryanikyTest.API.Controllers;

[ApiController]
[Route("api/sellers")]
public class SellerController : ControllerBase
{
    private readonly IMediator _mediator;

    public SellerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<SellerLookupDto>>> GetAllSellers(
        string? search, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetAllSellersQuery(search, new Page(page, pageSize));
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }    

    [HttpPost]
    public async Task<ActionResult<SellerCreatedDto>> CreateSeller(
        [FromForm] CreateSellerCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(CreateSeller), response);
    }

    [HttpGet("{sellerId}")]
    public async Task<ActionResult<SellerDto>> GetSellerById(
        Guid sellerId, CancellationToken cancellationToken)
    {
        var query = new GetSellerByIdQuery(sellerId);
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{sellerId}")]
    public async Task<ActionResult> DeleteSeller(
        Guid sellerId, CancellationToken cancellationToken)
    {
        var command = new DeleteSellerCommand(sellerId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}