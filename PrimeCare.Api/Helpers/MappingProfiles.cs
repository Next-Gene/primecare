using AutoMapper;
using PrimeCare.Api.Dtos;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>();
    }
}
