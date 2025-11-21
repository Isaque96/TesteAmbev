using Ambev.DeveloperEvaluation.Domain.Messaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductHandler(IProductRepository productRepository, IBus bus) : IRequestHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var deleted = await productRepository.DeleteAsync(request.Id, cancellationToken);
        
        await bus.Publish(new LogMessage("DeleteProduct", deleted));

        return !deleted ?
            throw new KeyNotFoundException($"Product with id {request.Id} was not found.") :
            new DeleteProductResult { Deleted = true };
    }
}