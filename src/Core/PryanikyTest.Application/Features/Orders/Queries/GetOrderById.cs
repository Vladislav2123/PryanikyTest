using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Features.Orders.Dto;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto>;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetOrderByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .FindAsync(request.OrderId, cancellationToken);

        if (order == null) throw new EntityNotFoundException(nameof(Order), request.OrderId);

        return _mapper.Map<OrderDto>(order);
    }
}
