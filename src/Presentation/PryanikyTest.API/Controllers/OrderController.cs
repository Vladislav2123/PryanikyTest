using PryanikyTest.Application.Features.Orders.Commands;
using PryanikyTest.Application.Features.Orders.Queries;
using PryanikyTest.Application.Features.Orders.Dto;
using PryanikyTest.Application.Pagination;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace PryanikyTest.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<OrderLookupDto>>> GetAllOrders(
        string? sortColumn, string? sortOrder,
        int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetAllOrdersQuery(sortColumn, sortOrder, new Page(page, pageSize));
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderCreatedDto>> CreateOrder(
        [FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(CreateOrder), response);
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(
        Guid orderId, CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(orderId);
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }


    [HttpDelete("{orderId}")]
    public async Task<ActionResult> DeleteOrder(
        Guid orderId, CancellationToken cancellationToken)
    {
        var command = new DeleteOrderCommand(orderId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{orderId}/status")]
    public async Task<ActionResult> UpdateOrderStatus(
        [FromForm] UpdateOrderStatusCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
