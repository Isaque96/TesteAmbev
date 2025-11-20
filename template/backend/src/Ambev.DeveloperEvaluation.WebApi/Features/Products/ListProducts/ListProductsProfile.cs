using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// Profile for mapping ListProducts feature
/// </summary>
public class ListProductsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListProducts feature
    /// </summary>
    public ListProductsProfile()
    {
        CreateMap<ListProductsRequest, ListProductsQuery>();
        CreateMap<Product, ListProductsResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<Rating, RatingDto>().ReverseMap();
    }
}
