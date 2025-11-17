using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class CartValidator : AbstractValidator<Cart>
{
    public CartValidator()
    {
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("UserId is required.");

        RuleFor(c => c.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Cart date cannot be in the future.");

        RuleFor(c => c.CartItems)
            .NotNull().WithMessage("Cart must have at least one product.")
            .Must(items => items.Any())
            .WithMessage("Cart must have at least one product.");

        RuleForEach(c => c.CartItems)
            .SetValidator(new CartItemValidator());
    }
}