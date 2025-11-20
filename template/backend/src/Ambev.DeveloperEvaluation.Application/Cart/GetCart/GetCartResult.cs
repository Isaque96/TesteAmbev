namespace Ambev.DeveloperEvaluation.Application.Cart.GetCart;

public class GetCartResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public IList<GetCartProductItem> Products { get; set; } = new List<GetCartProductItem>();
}

public class GetCartProductItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}