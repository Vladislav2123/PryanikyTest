using Microsoft.EntityFrameworkCore;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Features.Sellers.Dto;
using PryanikyTest.Application.Pagination;
using PryanikyTest.Domain.Entities;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Sellers.Queries;

public record GetAllSellersQuery(
    string? SearchTerms,
    Page Page
) : IRequest<PagedList<SellerLookupDto>>;

public class GetAllSellersHandler : IRequestHandler<GetAllSellersQuery, PagedList<SellerLookupDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllSellersHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedList<SellerLookupDto>> Handle(GetAllSellersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Seller> sellers = _dbContext.Sellers
            .Include(seller => seller.Products);

        if(string.IsNullOrEmpty(request.SearchTerms) == false)
        {
            sellers = sellers
                .Where(seller => seller.Name.Contains(request.SearchTerms)); 
        }

        sellers.OrderByDescending(seller => seller.Products.Count);

        var mappedSellers = _mapper.Map<List<SellerLookupDto>>(await sellers.ToListAsync(cancellationToken));
        return PagedList<SellerLookupDto>.Create(mappedSellers, request.Page);
    }
}
