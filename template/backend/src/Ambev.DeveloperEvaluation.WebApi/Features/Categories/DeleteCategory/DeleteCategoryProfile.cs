using Ambev.DeveloperEvaluation.Application.Categories.DeleteCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.DeleteCategory;

/// <summary>
/// Profile for mapping DeleteCategory feature requests to commands
/// </summary>
public class DeleteCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteCategory feature
    /// </summary>
    public DeleteCategoryProfile()
    {
        CreateMap<Guid, DeleteCategoryCommand>()
            .ConstructUsing(id => new DeleteCategoryCommand { Id = id });
    }
}