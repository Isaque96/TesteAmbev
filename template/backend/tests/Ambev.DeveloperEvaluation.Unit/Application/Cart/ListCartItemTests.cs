using Ambev.DeveloperEvaluation.Application.Cart.ListCarts;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class ListCartItemTests
{
    [Fact(DisplayName = "This is a placeholder test for ListCartItemTests")]
    public void PlaceholderTest()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var date = DateTime.UtcNow;
        var products = new List<ListCartsProductItem>
        {
            new ListCartsProductItem
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2
            }
        };

        var listCart = new ListCartsItem
        {
            Id = id,
            UserId = userId,
            Date = date,
            Products = products
        };

        Assert.Equal(id, listCart.Id);
        Assert.Equal(userId, listCart.UserId);
        Assert.Equal(date, listCart.Date);
        Assert.Equal(products, listCart.Products);
    }
}