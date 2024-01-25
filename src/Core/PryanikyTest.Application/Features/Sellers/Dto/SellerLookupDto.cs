using AutoMapper;
using PryanikyTest.Application.Mapping;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Features.Sellers.Dto;

public class SellerLookupDto : IMappping
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Seller, SellerLookupDto>();
    }
}
