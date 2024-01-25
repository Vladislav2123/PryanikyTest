using AutoMapper;
using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Features.Orders.Dto;

public class ProductOrderDto : IMappping
{
    public Guid ProductId { get; set; }
    public int Amount {get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<ProductOrder, ProductOrderDto>();
    }
}
