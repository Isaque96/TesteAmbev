using Ambev.DeveloperEvaluation.Application.Categories.GetCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.GetCategory;

/// <summary>
/// Profile for mapping GetCategory feature requests to commands
/// </summary>
public class GetCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCategory feature
    /// </summary>
    public GetCategoryProfile()
    {
        CreateMap<Guid, GetCategoryQuery>()
            .ConstructUsing(id => new GetCategoryQuery { Id = id });
        
        CreateMap<GetCategoryResult, GetCategoryResponse>();
    }
}