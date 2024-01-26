using PryanikyTest.Application.Features.Orders.Dto;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Pagination;
using PryanikyTest.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using PryanikyTest.Domain.Entities;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Orders.Queries;

public record GetOrdersByCustomerIdQuery(
    Guid CustomerId, 
    Page Page
) : IRequest<PagedList<OrderLookupDto>>;

public class GetOrderByCustomerIdHandler : IRequestHandler<GetOrdersByCustomerIdQuery, PagedList<OrderLookupDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetOrderByCustomerIdHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedList<OrderLookupDto>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        // Getting customer

        if(await _dbContext.Customers
            .AnyAsync(customer => customer.Id == request.CustomerId, cancellationToken) == false)
                throw new EntityNotFoundException(nameof(Customer), request.CustomerId);

        // Sorting orders
        IQueryable<Order> customerOrders = _dbContext.Orders
            .Include(order => order.ProductOrders)
            .Where(order => order.CustomerId == request.CustomerId);
        customerOrders = customerOrders.OrderByDescending(order => order.CreationDate);

        // Response
        var mappedOrders = _mapper.Map<List<OrderLookupDto>>(customerOrders.ToList());
        return PagedList<OrderLookupDto>.Create(mappedOrders, request.Page);
    }
}
