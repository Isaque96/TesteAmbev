namespace Ambev.DeveloperEvaluation.Application.Cart.ListCarts;

public class ListCartsResult
{
    public IEnumerable<ListCartsItem> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}