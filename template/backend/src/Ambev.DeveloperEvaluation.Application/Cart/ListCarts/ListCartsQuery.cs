using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.ListCarts;

public class ListCartsQuery : IRequest<ListCartsResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; } // "id desc, userId asc"
}