namespace Ambev.DeveloperEvaluation.Application.Cart.CreateCart;

public class CreateCartResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CreateCartProductItem> Products { get; set; } = new();
    public DateTime CreatedAt { get; set; }

}