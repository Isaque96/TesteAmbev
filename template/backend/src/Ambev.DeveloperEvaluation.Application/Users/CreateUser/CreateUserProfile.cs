using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Profile for mapping between User entity and CreateUserResponse
/// </summary>
public class CreateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser operation
    /// </summary>
    public CreateUserProfile()
    {
        CreateMap<CreateUserCommand, User>()
            .ForPath(dest => dest.Name.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForPath(dest => dest.Name.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
            .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
            .ForPath(dest => dest.Address.Number, opt => opt.MapFrom(src => int.Parse(src.Number)))
            .ForPath(dest => dest.Address.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
            .ForPath(dest => dest.Address.Geolocation.Lat, opt => opt.MapFrom(src => src.Latitude))
            .ForPath(dest => dest.Address.Geolocation.Long, opt => opt.MapFrom(src => src.Longitude));
        CreateMap<User, CreateUserResult>();
    }
}
