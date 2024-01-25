using AutoMapper;
using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Features.Products.Dto;

public class ProductLookupDto : IMappping
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int TotalOrders { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>();
    }
}
