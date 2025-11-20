using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Categories.GetCategory;

public class GetCategoryProfile : Profile
{
    public GetCategoryProfile()
    {
        CreateMap<Category, GetCategoryResult>();
    }
}