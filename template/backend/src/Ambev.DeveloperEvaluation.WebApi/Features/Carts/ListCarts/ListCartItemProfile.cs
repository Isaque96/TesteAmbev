using Ambev.DeveloperEvaluation.Application.Cart.ListCarts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;

/// <summary>
/// Profile for mapping ListCarts feature
/// </summary>
public class ListCartsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListCarts feature
    /// </summary>
    public ListCartsProfile()
    {
        CreateMap<ListCartsRequest, ListCartsQuery>();
        CreateMap<Cart, ListCartsResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.CartItems))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.CalculateTotal()));
        CreateMap<CartItem, CartItemDto>();
    }
}