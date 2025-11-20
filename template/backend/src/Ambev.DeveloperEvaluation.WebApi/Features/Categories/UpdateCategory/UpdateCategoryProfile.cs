using Ambev.DeveloperEvaluation.Application.Categories.UpdateCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory;

/// <summary>
/// Profile for mapping UpdateCategory feature requests to commands
/// </summary>
public class UpdateCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateCategory feature
    /// </summary>
    public UpdateCategoryProfile()
    {
        CreateMap<UpdateCategoryRequest, UpdateCategoryCommand>();
        CreateMap<UpdateCategoryResult, UpdateCategoryResponse>();
    }
}