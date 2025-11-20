using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.ListProductsByCategory;

public class ListProductsByCategoryHandler(IProductRepository productRepository, IMapper mapper)
    : IRequestHandler<ListProductsByCategoryQuery, ListProductsByCategoryResult>
{
    public async Task<ListProductsByCategoryResult> Handle(ListProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var (items, totalItems) = await productRepository.GetProductsByCategoryAsync(
            request.CategoryName,
            request.Page,
            request.Size,
            request.Order,
            cancellationToken
        );

        var data = mapper.Map<IEnumerable<ListProductsByCategoryItem>>(items);
        var totalPages = (int)Math.Ceiling(totalItems / (double)request.Size);

        return new ListProductsByCategoryResult
        {
            Data = data,
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = totalPages
        };
    }
}