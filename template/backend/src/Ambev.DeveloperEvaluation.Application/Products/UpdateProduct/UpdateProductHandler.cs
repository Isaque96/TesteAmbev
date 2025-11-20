using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
    : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existing == null)
            throw new KeyNotFoundException($"Product with id {command.Id} was not found.");

        // Atualiza os campos
        existing.Title = command.Title;
        existing.UpdatePrice(command.Price);
        existing.Description = command.Description;
        existing.Category = await categoryRepository.GetCategoryByNameAsync(command.Category, cancellationToken);
        existing.Image = command.Image;
        existing.Rating.Rate = command.Rate;
        existing.Rating.Count = command.Count;

        var updated = await productRepository.UpdateAsync(existing, cancellationToken);
        return mapper.Map<UpdateProductResult>(updated);
    }
}