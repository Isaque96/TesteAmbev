using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.GetCart;

public class GetCartHandler(ICartRepository cartRepository, IMapper mapper)
    : IRequestHandler<GetCartQuery, GetCartResult>
{
    public async Task<GetCartResult> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByIdAsync(request.Id, cancellationToken);

        return cart == null ?
            throw new KeyNotFoundException($"Cart with id {request.Id} was not found.") :
            mapper.Map<GetCartResult>(cart);
    }
}