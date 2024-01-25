using AutoMapper;
using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Features.Orders.Dto;

public class OrderLookupDto : IMappping
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public int ProductOrdersAmount { get; set; }
    public bool Completed { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Order, OrderLookupDto>()
            .ForMember(
                dest => dest.ProductOrdersAmount,
                opt => opt.MapFrom(
                    src => src.ProductOrders.Count));
    }
}
