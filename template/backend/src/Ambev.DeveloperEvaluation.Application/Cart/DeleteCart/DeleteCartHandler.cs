using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.DeleteCart;

public class DeleteCartHandler(ICartRepository cartRepository) : IRequestHandler<DeleteCartCommand, DeleteCartResult>
{
    public async Task<DeleteCartResult> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var deleted = await cartRepository.DeleteAsync(request.Id, cancellationToken);

        return !deleted ?
            throw new KeyNotFoundException($"Cart with id {request.Id} was not found.") :
            new DeleteCartResult { Deleted = true };
    }
}