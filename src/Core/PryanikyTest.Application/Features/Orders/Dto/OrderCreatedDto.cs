using AutoMapper;
using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Features.Orders.Dto;

public class OrderCreatedDto : IMappping
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public int ProductOrdersAmount { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Order, OrderCreatedDto>()
            .ForMember(
                dest => dest.ProductOrdersAmount, 
                opt => opt.MapFrom(
                    src => src.ProductOrders.Count));
    }
}
