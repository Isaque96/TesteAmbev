using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Messaging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Handler for processing CreateProductCommand requests
/// </summary>
public class CreateProductHandler(ICategoryRepository categoryRepository, IProductRepository productRepository, IMapper mapper, IBus bus)
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

        await bus.Publish(new LogMessage("CreateProduct", created));
        
        return mapper.Map<CreateProductResult>(created);
    }
}