using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Cart.CreateCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<CreateCartProductItem, CartItem>()
            .ForMember(dest => dest.CartId, opt => opt.Ignore()); // será setado pelo Cart/EF

        CreateMap<CreateCartCommand, Domain.Entities.Cart>()
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.Products));

        CreateMap<Domain.Entities.Cart, CreateCartResult>();
    }
}