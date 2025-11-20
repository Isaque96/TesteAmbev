using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.UpdateCategory;

public class UpdateCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);
        if (category == null)
            throw new KeyNotFoundException($"Category with id {command.Id} was not found.");

        // Garantir nome único (opcional)
        var exists = await categoryRepository.ExistsByNameExceptIdAsync(command.Name, command.Id, cancellationToken);
        if (exists)
            throw new ValidationException($"Category '{command.Name}' already exists.");

        category.Name = command.Name;
        category.Description = command.Description;

        var updated = await categoryRepository.UpdateAsync(category, cancellationToken);

        return mapper.Map<UpdateCategoryResult>(updated);
    }
}