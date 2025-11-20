using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Validator for UpdateCartRequest
/// </summary>
public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateCartRequest
    /// </summary>
    public UpdateCartRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cart ID is required");

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