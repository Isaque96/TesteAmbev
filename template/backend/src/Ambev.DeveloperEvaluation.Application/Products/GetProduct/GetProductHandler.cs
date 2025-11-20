using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler(IProductRepository productRepository, IMapper mapper)
    : IRequestHandler<GetProductQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        return product == null ?
            throw new KeyNotFoundException($"Product with id {request.Id} was not found.") :
            mapper.Map<GetProductResult>(product);
    }
}