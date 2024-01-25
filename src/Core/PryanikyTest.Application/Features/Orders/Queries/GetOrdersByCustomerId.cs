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
        var customer = await _dbContext.Customers
            .Include(customer => customer.Orders)
            .FirstOrDefaultAsync(customer => customer.Id == request.CustomerId, cancellationToken);

        if(customer == null) throw new EntityNotFoundException(nameof(Customer), request.CustomerId);

        // Sorting orders
        IQueryable<Order> customerOrders = customer.Orders.AsQueryable();
        customerOrders = customerOrders.OrderByDescending(order => order.CreationDate);

        // Response
        var mappedOrders = _mapper.Map<List<OrderLookupDto>>(await customerOrders.ToListAsync(cancellationToken));
        return PagedList<OrderLookupDto>.Create(mappedOrders, request.Page);
    }
}
