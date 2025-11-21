using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Categories.CreateCategory;

public class CreateCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    : IRequestHandler<CreateCategoryCommand, CreateCategoryResult>
{
    public async Task<CreateCategoryResult> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateCategoryCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Evita duplicidade de nome
        var exists = await categoryRepository.ExistsByNameAsync(command.Name, cancellationToken);
        if (exists)
            throw new ValidationException($"CategoryName '{command.Name}' already exists.");

        var category = mapper.Map<Category>(command);

        // CreatedAt já é setado no ctor da entidade, mas se quiser reforçar:
        if (category.CreatedAt == default)
            category.CreatedAt = DateTime.UtcNow;

        var created = await categoryRepository.CreateAsync(category, cancellationToken);

        return mapper.Map<CreateCategoryResult>(created);
    }
}