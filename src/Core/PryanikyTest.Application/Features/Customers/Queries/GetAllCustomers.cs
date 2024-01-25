using PryanikyTest.Application.Features.Customers.Dto;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Pagination;
using Microsoft.EntityFrameworkCore;
using PryanikyTest.Domain.Entities;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Customers.Queries;

public record GetAllCustomersQuery(Page Page) : IRequest<PagedList<CustomerLookupDto>>;

public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, PagedList<CustomerLookupDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllCustomersHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedList<CustomerLookupDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        List<Customer> customers = await _dbContext.Customers.ToListAsync();
        
        var mappedCustomers = _mapper.Map<List<CustomerLookupDto>>(customers);

        return PagedList<CustomerLookupDto>.Create(mappedCustomers, request.Page);
    }
}
