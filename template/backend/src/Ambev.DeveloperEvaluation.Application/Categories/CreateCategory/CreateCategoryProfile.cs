using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;

public class CreateCategoryProfile : Profile
{
    public CreateCategoryProfile()
    {
        CreateMap<CreateCategoryCommand, Category>();
        CreateMap<Category, CreateCategoryResult>();
    }
}