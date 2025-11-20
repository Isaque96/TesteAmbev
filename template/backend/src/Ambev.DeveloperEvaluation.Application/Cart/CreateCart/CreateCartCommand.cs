using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.CreateCart;

public class CreateCartCommand : IRequest<CreateCartResult>
{
    public Guid UserId { get; set; }

    // A doc fala "string (date)"; vamos receber DateTime aqui
    public DateTime Date { get; set; }

    public IList<CreateCartProductItem> Products { get; set; } = new List<CreateCartProductItem>();

    public ValidationResultDetail Validate()
    {
        var validator = new CreateCartCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

public class CreateCartProductItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}