using PryanikyTest.Domain.Entities;
using PryanikyTest.Application.Mapping;
using AutoMapper;

namespace PryanikyTest.Application.Features.Customers.Dto;

public class CustomerDto : IMappping
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int OrdersCount { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Customer, CustomerDto>()
            .ForMember(
                dest => dest.OrdersCount,
                opt => opt.MapFrom(
                    src => src.Orders.Count));
    }
}
