using Ambev.DeveloperEvaluation.Application.Categories.ListCategories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories;

/// <summary>
/// Profile for mapping ListCategories feature
/// </summary>
public class ListCategoriesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListCategories feature
    /// </summary>
    public ListCategoriesProfile()
    {
        CreateMap<ListCategoriesRequest, ListCategoriesQuery>();
        CreateMap<Category, ListCategoriesResponse>();
    }
}