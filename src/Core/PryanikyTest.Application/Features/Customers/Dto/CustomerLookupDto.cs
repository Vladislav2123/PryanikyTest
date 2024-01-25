using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;
using AutoMapper;

namespace PryanikyTest.Application.Features.Customers.Dto;

public class CustomerLookupDto : IMappping
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Customer, CustomerLookupDto>();
    }
}
