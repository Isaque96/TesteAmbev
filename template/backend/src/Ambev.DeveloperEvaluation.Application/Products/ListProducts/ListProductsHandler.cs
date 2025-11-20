using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsHandler(IProductRepository productRepository, IMapper mapper)
    : IRequestHandler<ListProductsQuery, ListProductsResult>
{
    public async Task<ListProductsResult> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalItems) = await productRepository.GetProductsPaginatedAsync(
            request.Page,
            request.Size,
            request.Order,
            request.Title,
            request.Category,
            request.MaxPrice,
            request.MinPrice,
            cancellationToken
        );
        var data = mapper.Map<IEnumerable<ListProductsItem>>(items);
        var totalPages = (int)Math.Ceiling(totalItems / (double)request.Size);

        return new ListProductsResult
        {
            Data = data,
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }
}