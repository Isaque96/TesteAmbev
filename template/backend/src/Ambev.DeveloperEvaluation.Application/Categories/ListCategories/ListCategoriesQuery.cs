using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.ListCategories;

public class ListCategoriesQuery : IRequest<IEnumerable<string>> { }