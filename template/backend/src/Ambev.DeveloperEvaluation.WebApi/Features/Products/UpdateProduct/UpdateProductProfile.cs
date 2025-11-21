using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Profile for mapping UpdateProduct feature requests to commands
/// </summary>
public class UpdateProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateProduct feature
    /// </summary>
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<UpdateProductResult, UpdateProductResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Id));
    }
}