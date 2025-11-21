using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.DeleteCategory;

public class DeleteCategoryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<DeleteCategoryCommand, DeleteCategoryResult>
{
    public async Task<DeleteCategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var deleted = await categoryRepository.DeleteAsync(request.Id, cancellationToken);

        return !deleted ?
            throw new KeyNotFoundException($"CategoryName with id {request.Id} was not found.") :
            new DeleteCategoryResult { Deleted = true };
    }
}