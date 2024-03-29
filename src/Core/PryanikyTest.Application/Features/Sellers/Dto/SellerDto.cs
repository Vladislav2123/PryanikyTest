﻿using PryanikyTest.Domain.Entities;
using PryanikyTest.Application.Mapping;
using AutoMapper;

namespace PryanikyTest.Application.Features.Sellers.Dto;

public class SellerDto : IMappping
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int ProductsAmount { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<Seller, SellerDto>()
            .ForMember(
                dest => dest.ProductsAmount,
                opt => opt.MapFrom(
                    src => src.Products.Count));
    }
}
