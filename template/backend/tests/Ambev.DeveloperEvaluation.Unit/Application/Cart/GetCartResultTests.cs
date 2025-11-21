using Ambev.DeveloperEvaluation.Application.Cart.GetCart;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Cart;

public class GetCartResultTests
{
    [Fact(DisplayName = "Testing instance of class GetCartResult")]
    public void Test_GetCartResult_Instance()
    {
        var result = new GetCartResult
        {
            Date =  DateTime.MinValue,
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Products = [
                new GetCartProductItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2
                }
            ]
        };

        Assert.Equal(DateTime.MinValue, result.Date);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.Single(result.Products);
        Assert.Equal(2, result.Products[0].Quantity);
    }
}