using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Handler for processing CreateProductCommand requests
/// </summary>
public class CreateProductHandler(ICategoryRepository categoryRepository, IProductRepository productRepository, IMapper mapper)
    : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = await categoryRepository.GetCategoryByNameAsync(command.CategoryName, cancellationToken);
        
        var product = mapper.Map<Product>(command);
        product.Category = category;
        
        var created = await productRepository.CreateAsync(product, cancellationToken);

        return mapper.Map<CreateProductResult>(created);
    }
}