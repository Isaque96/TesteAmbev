using Ambev.DeveloperEvaluation.Application.Cart.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Profile for mapping CreateCart feature requests to commands
/// </summary>
public class CreateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateCart feature
    /// </summary>
    public CreateCartProfile()
    {
        CreateMap<CreateCartRequest, CreateCartCommand>();
        CreateMap<CreateCartResult, CreateCartResponse>();
        CreateMap<CartItemDto, CreateCartProductItem>().ReverseMap();
    }
}