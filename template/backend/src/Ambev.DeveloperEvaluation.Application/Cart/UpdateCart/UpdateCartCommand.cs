using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.UpdateCart;

public class UpdateCartCommand : IRequest<UpdateCartResult>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public IList<UpdateCartProductItem> Products { get; set; } = new List<UpdateCartProductItem>();

    public ValidationResultDetail Validate()
    {
        var validator = new UpdateCartCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

public class UpdateCartProductItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}