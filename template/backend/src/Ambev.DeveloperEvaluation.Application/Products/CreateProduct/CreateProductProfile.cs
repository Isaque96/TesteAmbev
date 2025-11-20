using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Profile for mapping between Product entity and CreateProduct models
/// </summary>
public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        // Command -> Entity
        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new Rating
            {
                Rate = src.Rate,
                Count = src.Count
            }));

        // Entity -> Result
        CreateMap<Product, CreateProductResult>();
    }
}