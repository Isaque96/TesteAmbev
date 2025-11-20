using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;

public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
{
    public UpdateCartCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.Date).NotEmpty();

        RuleFor(c => c.Products)
            .NotNull()
            .NotEmpty()
            .WithMessage("Cart must have at least one product.");

        RuleForEach(c => c.Products).ChildRules(p =>
        {
            p.RuleFor(x => x.ProductId).NotEmpty();
            p.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(20);
        });
    }
}