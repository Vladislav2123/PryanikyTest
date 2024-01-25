using AutoMapper;
using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Features.Orders.Dto;

public class OrderDto : IMappping
{
    public Guid Id { get; set; }
    public ICollection<ProductOrderDto> ProductOrders { get; set; }
    public bool Completed { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid CustomerId { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Order, OrderDto>();
    }
}
