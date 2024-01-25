using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Features.Sellers.Dto;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Sellers.Queries;

public record GetSellerByIdQuery(Guid SellerId) : IRequest<SellerDto>;

public class GetSellerByIdHandler : IRequestHandler<GetSellerByIdQuery, SellerDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetSellerByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SellerDto> Handle(GetSellerByIdQuery request, CancellationToken cancellationToken)
    {
        var seller = await _dbContext.Sellers
            .Include(seller => seller.Products)
            .FirstOrDefaultAsync(seller => seller.Id == request.SellerId, cancellationToken);

        if(seller == null) throw new EntityNotFoundException(nameof(Seller), request.SellerId);

        return _mapper.Map<SellerDto>(seller);
    }
}
