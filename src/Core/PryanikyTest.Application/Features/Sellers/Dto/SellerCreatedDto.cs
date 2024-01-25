using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;
using AutoMapper;

namespace PryanikyTest.Application.Features.Sellers.Dto;

public class SellerCreatedDto : IMappping
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Seller, SellerCreatedDto>();
    }
}
