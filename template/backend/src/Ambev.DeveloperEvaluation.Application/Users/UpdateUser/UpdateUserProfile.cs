using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

public class UpdateUserProfile : Profile
{
    public UpdateUserProfile()
    {
        // Command → User (usaremos misto; algumas coisas vamos aplicar na mão)
        CreateMap<UpdateUserCommand, User>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore());

        // User -> Result
        CreateMap<User, UpdateUserResult>();
    }
}