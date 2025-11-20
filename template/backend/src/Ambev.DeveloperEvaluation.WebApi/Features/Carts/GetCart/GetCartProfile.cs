using Ambev.DeveloperEvaluation.Application.Cart.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// Profile for mapping GetCart feature requests to commands
/// </summary>
public class GetCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCart feature
    /// </summary>
    public GetCartProfile()
    {
        CreateMap<Guid, GetCartQuery>()
            .ConstructUsing(id => new GetCartQuery { Id = id });
        
        CreateMap<GetCartResult, GetCartResponse>();
        CreateMap<GetCartProductItem, CartItemDto>();
    }
}