using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Validator for UpdateProductRequest
/// </summary>
public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateProductRequest
    /// </summary>
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Product title is required")
            .MaximumLength(200)
            .WithMessage("Product title must not exceed 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Product price must be greater than zero");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Product description is required")
            .MaximumLength(1000)
            .WithMessage("Product description must not exceed 1000 characters");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryName ID is required");

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage("Product image URL is required");

        RuleFor(x => x.Rating.Rate)
            .InclusiveBetween(0, 5)
            .WithMessage("Rating must be between 0 and 5");

        RuleFor(x => x.Rating.Count)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Rating count must be non-negative");
    }
}