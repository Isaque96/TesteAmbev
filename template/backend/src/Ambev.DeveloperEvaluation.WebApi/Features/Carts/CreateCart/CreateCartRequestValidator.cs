using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Validator for CreateCartRequest
/// </summary>
public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
{
    /// <summary>
    /// Initializes validation rules for CreateCartRequest
    /// </summary>
    public CreateCartRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Cart date is required");

        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage("Cart must contain at least one product");

        RuleForEach(x => x.Products).ChildRules(product =>
        {
            product.RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product ID is required");

            product.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero")
                .LessThanOrEqualTo(20)
                .WithMessage("Cannot add more than 20 identical items");
        });
    }
}