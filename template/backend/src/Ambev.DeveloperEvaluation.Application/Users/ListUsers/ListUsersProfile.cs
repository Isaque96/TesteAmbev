using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

public class ListUsersProfile : Profile
{
    public ListUsersProfile()
    {
        CreateMap<User, ListUsersItem>()
            .ForMember(dest => dest.FirstName,opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Address.Number))
            .ForMember(dest => dest.Zipcode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Address.Geolocation.Lat))
            .ForMember(dest => dest.Long, opt => opt.MapFrom(src => src.Address.Geolocation.Long));
    }
}