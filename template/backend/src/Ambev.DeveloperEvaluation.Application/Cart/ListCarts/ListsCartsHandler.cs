using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.ListCarts;

public class ListCartsHandler(ICartRepository cartRepository, IMapper mapper)
    : IRequestHandler<ListCartsQuery, ListCartsResult>
{
    public async Task<ListCartsResult> Handle(ListCartsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalItems) = await cartRepository.GetCartPaginatedAsync(
            request.Page,
            request.Size,
            request.Order,
            cancellationToken
        );

        var data = mapper.Map<IEnumerable<ListCartsItem>>(items);
        var totalPages = (int)Math.Ceiling(totalItems / (double)request.Size);

        return new ListCartsResult
        {
            Data = data,
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }
}