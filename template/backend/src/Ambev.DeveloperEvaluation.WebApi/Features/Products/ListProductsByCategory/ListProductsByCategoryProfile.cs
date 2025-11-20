using Ambev.DeveloperEvaluation.Application.Categories.ListProductsByCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductsByCategory;

public class ListProductsByCategoryProfile : Profile
{
    public ListProductsByCategoryProfile()
    {
        CreateMap<ListProductsByCategoryRequest, ListProductsByCategoryQuery>();
        CreateMap<ListProductsByCategoryResult, ListProductsByCategoryResponse>();
    }
}
