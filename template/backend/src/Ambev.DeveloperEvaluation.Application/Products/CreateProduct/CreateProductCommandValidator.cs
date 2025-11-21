using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Validator for CreateProductCommand that defines validation rules for product creation.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .Length(3, 150);

        RuleFor(p => p.Price)
            .GreaterThan(0);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(p => p.CategoryName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(p => p.Image)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(p => p.Rate)
            .InclusiveBetween(0, 5);

        RuleFor(p => p.Count)
            .GreaterThanOrEqualTo(0);
    }
}