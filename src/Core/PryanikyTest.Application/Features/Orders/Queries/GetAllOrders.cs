using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Features.Orders.Dto;
using PryanikyTest.Application.Pagination;
using Microsoft.EntityFrameworkCore;
using PryanikyTest.Domain.Entities;
using System.Linq.Expressions;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Orders.Queries;

public record GetAllOrdersQuery(
    string? SortColumn,
    string? SortOrder,
    Page Page
) : IRequest<PagedList<OrderLookupDto>>;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, PagedList<OrderLookupDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllOrdersHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedList<OrderLookupDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Order> orders = _dbContext.Orders
            .Include(order => order.ProductOrders);

        var sortColumnExpression = GetSortColumnExpression(request);
        if(request.SortOrder?.ToLower() == "asc") orders = orders.OrderBy(sortColumnExpression);
        else orders = orders.OrderByDescending(sortColumnExpression);

        var mappedOrders = _mapper.Map<List<OrderLookupDto>>(await orders.ToListAsync(cancellationToken));
        return PagedList<OrderLookupDto>.Create(mappedOrders, request.Page);
    }

    private Expression<Func<Order, object>> GetSortColumnExpression(GetAllOrdersQuery request) =>
        request.SortOrder?.ToLower() switch
        {
            "products" => order => order.ProductOrders.Sum(productOrder => productOrder.Amount),
            "date" => order => order.CreationDate,
            _ => order => order.CreationDate
        };
}
