using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.ListCategories;

public class ListCategoriesHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<ListCategoriesQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await categoryRepository.GetCategoriesAsync(cancellationToken);
    }
}