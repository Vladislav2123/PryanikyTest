using PryanikyTest.Application.Features.Products.Dto;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Domain.Exceptions;
using PryanikyTest.Domain.Entities;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid ProductId) : IRequest<ProductDto>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .FindAsync(request.ProductId, cancellationToken);

        if(product == null) throw new EntityNotFoundException(nameof(Product), request.ProductId);

        return _mapper.Map<ProductDto>(product);
    }
}
