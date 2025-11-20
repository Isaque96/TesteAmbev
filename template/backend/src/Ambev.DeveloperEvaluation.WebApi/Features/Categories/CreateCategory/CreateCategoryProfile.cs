using Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory;

/// <summary>
/// Profile for mapping CreateCategory feature requests to commands
/// </summary>
public class CreateCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateCategory feature
    /// </summary>
    public CreateCategoryProfile()
    {
        CreateMap<CreateCategoryRequest, CreateCategoryCommand>();
        CreateMap<CreateCategoryResult, CreateCategoryResponse>();
    }
}