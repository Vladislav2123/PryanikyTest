using AutoMapper;
using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Features.Customers.Dto;

public class CustomerCreatedDto : IMappping
{
    public Guid Id {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Customer, CustomerCreatedDto>();
    }
}
