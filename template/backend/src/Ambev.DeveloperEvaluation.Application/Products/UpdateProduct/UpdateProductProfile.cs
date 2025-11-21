using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductCommand, Product>()
            //.ForMember(dest => dest.Category.Name, opt => opt.MapFrom(src => src.CategoryName))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new Rating
            {
                Rate = src.Rate,
                Count = src.Count
            }));

        CreateMap<Product, UpdateProductResult>();
    }
}