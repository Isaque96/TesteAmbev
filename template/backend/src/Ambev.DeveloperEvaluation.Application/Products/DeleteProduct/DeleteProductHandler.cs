using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var deleted = await productRepository.DeleteAsync(request.Id, cancellationToken);

        return !deleted ?
            throw new KeyNotFoundException($"Product with id {request.Id} was not found.") :
            new DeleteProductResult { Deleted = true };
    }
}