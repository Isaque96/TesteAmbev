using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

public class ListUsersProfile : Profile
{
    public ListUsersProfile()
    {
        // WebApi Request -> Application Query
        CreateMap<ListUsersRequest, ListUsersQuery>();
        CreateMap<User, ListUsersResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}