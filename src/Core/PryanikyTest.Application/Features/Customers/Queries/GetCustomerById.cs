using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Features.Customers.Dto;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;

namespace PryanikyTest.Application.Features.Customers.Queries;

public record GetCustomerByIdCommand(Guid CustomerId) : IRequest<CustomerDto>;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdCommand, CustomerDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCustomerByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdCommand request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .Include(customer => customer.Orders)
            .FirstOrDefaultAsync(customer => customer.Id == request.CustomerId, cancellationToken);

        if(customer == null) throw new EntityNotFoundException(nameof(Customer), request.CustomerId);

        return _mapper.Map<CustomerDto>(customer);
    }
}
