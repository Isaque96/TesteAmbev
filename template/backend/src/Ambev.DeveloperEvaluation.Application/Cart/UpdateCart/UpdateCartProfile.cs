using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;

public class UpdateCartProfile : Profile
{
    public UpdateCartProfile()
    {
        CreateMap<UpdateCartProductItem, CartItem>()
            .ForMember(dest => dest.CartId, opt => opt.Ignore());

        CreateMap<UpdateCartCommand, Domain.Entities.Cart>()
            .ForMember(dest => dest.CartItems, opt => opt.Ignore()); // vamos mexer na mão

        CreateMap<Domain.Entities.Cart, UpdateCartResult>();
    }
}