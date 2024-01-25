using PryanikyTest.Application.Features.Customers.Commands;
using PryanikyTest.Application.Features.Customers.Queries;
using PryanikyTest.Application.Features.Orders.Queries;
using PryanikyTest.Application.Features.Customers.Dto;
using PryanikyTest.Application.Features.Orders.Dto;
using PryanikyTest.Application.Pagination;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace PryanikyTest.API.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<CustomerLookupDto>>> GetAllCustomers(
        int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetAllCustomersQuery(new Page(page, pageSize));
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerCreatedDto>> CreateCustomer(
        [FromForm] CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(CreateCustomer), response);
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(
        Guid customerId, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdQuery(customerId);
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{customerId}")]
    public async Task<ActionResult> DeleteCustomer(
        Guid customerId, CancellationToken cancellationToken)
    {
        var command = new DeleteCustomerCommand(customerId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("{customerId}/orders")]
    public async Task<ActionResult<PagedList<OrderLookupDto>>> GetCustomerOrders(
        Guid customerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = new GetOrdersByCustomerIdQuery(customerId, new Page(page, pageSize));
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }
}
