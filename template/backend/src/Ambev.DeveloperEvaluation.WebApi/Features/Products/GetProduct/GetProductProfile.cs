using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Profile for mapping GetProduct feature requests to commands
/// </summary>
public class GetProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProduct feature
    /// </summary>
    public GetProductProfile()
    {
        CreateMap<Guid, GetProductQuery>()
            .ConstructUsing(id => new GetProductQuery { Id = id });
        
        CreateMap<GetProductResult, GetProductResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        
        CreateMap<Rating, RatingDto>().ReverseMap();
    }
}