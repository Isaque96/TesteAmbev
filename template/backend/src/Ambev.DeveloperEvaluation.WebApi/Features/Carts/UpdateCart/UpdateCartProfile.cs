using Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Profile for mapping UpdateCart feature requests to commands
/// </summary>
public class UpdateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateCart feature
    /// </summary>
    public UpdateCartProfile()
    {
        CreateMap<UpdateCartRequest, UpdateCartCommand>();
        CreateMap<UpdateCartResult, UpdateCartResponse>();
        CreateMap<CartItemDto, UpdateCartProductItem>().ReverseMap();
    }
}