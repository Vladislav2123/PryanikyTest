using PryanikyTest.Application.Features.Products.Dto;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Pagination;
using PryanikyTest.Domain.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Products.Queries;

public record GetAllProductsQuery(
    string? SearchTerms,
    Guid? SellerId,
    string? SortColumn,
    string? SortOrder,
    Page Page
) : IRequest<PagedList<ProductLookupDto>>;

public class GetAllProductsHandler
    : IRequestHandler<GetAllProductsQuery, PagedList<ProductLookupDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedList<ProductLookupDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Product> products = _dbContext.Products;

        // Filtering
        if (string.IsNullOrEmpty(request.SearchTerms) == false)
        {
            products = products.Where(product =>
                product.Name.Contains(request.SearchTerms) ||
                product.Description.Contains(request.SearchTerms));
        }

        if (request.SellerId != null && request.SellerId != Guid.Empty)
        {
            products = products
                .Where(product => product.SellerId == request.SellerId);
        }

        // Sorting
        var sortColumnExpression = GetSortColumnExpression(request);
        if (request.SortOrder?.ToLower() == "asc") products = products.OrderBy(sortColumnExpression);
        else products = products.OrderByDescending(sortColumnExpression);

        // Response
        var mappedProducts = _mapper.Map<List<ProductLookupDto>>(products.ToList());
        return PagedList<ProductLookupDto>.Create(mappedProducts, request.Page);
    }

    private Expression<Func<Product, object>> GetSortColumnExpression(GetAllProductsQuery request) =>
        request.SortColumn?.ToLower() switch
        {
            "price" => product => product.Price,
            "orders" => product => product.TotalOrders,
            _ => product => product.TotalOrders
        };
}
