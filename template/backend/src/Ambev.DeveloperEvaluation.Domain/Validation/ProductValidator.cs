using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
            .MaximumLength(200).WithMessage("Title cannot be longer than 200 characters.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.Description)
            .MaximumLength(2000)
            .WithMessage("Description cannot be longer than 2000 characters.");

        RuleFor(p => p.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage("CategoryId is required.");

        RuleFor(p => p.Image)
            .NotEmpty().WithMessage("Image URL is required.")
            .Must(BeAValidUrl)
            .WithMessage("Image must be a valid URL.");

        RuleFor(p => p.Rating)
            .SetValidator(new RatingValidator());
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}