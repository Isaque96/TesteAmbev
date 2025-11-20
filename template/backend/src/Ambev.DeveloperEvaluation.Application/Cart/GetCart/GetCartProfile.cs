using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Cart.GetCart;

public class GetCartProfile : Profile
{
    public GetCartProfile()
    {
        CreateMap<CartItem, GetCartProductItem>();

        CreateMap<Domain.Entities.Cart, GetCartResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.CartItems));
    }
}