using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.GetCategory;

public class GetCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    : IRequestHandler<GetCategoryQuery, GetCategoryResult>
{
    public async Task<GetCategoryResult> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        return category == null ?
            throw new KeyNotFoundException($"Category with id {request.Id} was not found.") :
            mapper.Map<GetCategoryResult>(category);
    }
}