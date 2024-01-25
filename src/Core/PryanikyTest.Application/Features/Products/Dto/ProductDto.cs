using AutoMapper;
using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Features.Products.Dto;

public class ProductDto : IMappping
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public int OrdersAmount { get; set; }
    public DateTime CreationDate { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.OrdersAmount,
                opt => opt.MapFrom(
                    src => src.ProductOrders.Sum(x => x.Amount)));
    }
}
