using Ambev.DeveloperEvaluation.Application.Categories.ListProductsByCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductsByCategory;

public class ListProductsByCategoryProfile : Profile
{
    public ListProductsByCategoryProfile()
    {
        // Request -> Query
        CreateMap<ListProductsByCategoryRequest, ListProductsByCategoryQuery>();

        // Item (application) -> Item (response)
        CreateMap<ListProductsByCategoryItem, ListProductsByCategoryResponseItem>()
            .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => src.CategoryName));

        // Result (application) -> Response (WebApi)
        CreateMap<ListProductsByCategoryResult, ListProductsByCategoryResponse>();
    }
}
