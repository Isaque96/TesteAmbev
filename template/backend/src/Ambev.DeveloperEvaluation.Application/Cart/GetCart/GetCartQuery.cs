using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.GetCart;

public class GetCartQuery : IRequest<GetCartResult>
{
    public Guid Id { get; set; }
}