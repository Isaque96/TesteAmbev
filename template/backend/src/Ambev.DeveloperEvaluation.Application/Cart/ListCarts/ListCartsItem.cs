namespace Ambev.DeveloperEvaluation.Application.Cart.ListCarts;

public class ListCartsItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public IList<ListCartsProductItem> Products { get; set; } = new List<ListCartsProductItem>();
}

public class ListCartsProductItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}