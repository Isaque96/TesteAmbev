using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Cart.ListCarts;

public class ListCartsProfile : Profile
{
    public ListCartsProfile()
    {
        CreateMap<CartItem, ListCartsProductItem>();

        CreateMap<Domain.Entities.Cart, ListCartsItem>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.CartItems));
    }
}