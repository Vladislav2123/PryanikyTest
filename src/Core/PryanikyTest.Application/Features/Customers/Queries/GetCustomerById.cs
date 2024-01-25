using Microsoft.EntityFrameworkCore;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Features.Customers.Dto;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Customers.Queries;

public record GetCustomerByIdQuery(Guid CustomerId) : IRequest<CustomerDto>;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCustomerByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .Include(customer => customer.Orders)
            .FirstOrDefaultAsync(customer => customer.Id == request.CustomerId, cancellationToken);

        if(customer == null) throw new EntityNotFoundException(nameof(Customer), request.CustomerId);

        return _mapper.Map<CustomerDto>(customer);
    }
}
