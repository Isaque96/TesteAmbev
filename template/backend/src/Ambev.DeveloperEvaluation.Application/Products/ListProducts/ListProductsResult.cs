namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

public class ListProductsResult
{
    public IEnumerable<ListProductsItem> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}