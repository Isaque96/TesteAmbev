using Ambev.DeveloperEvaluation.Unit.Helpers;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Xunit;
// ajuste conforme seu namespace real

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Common;

public class PaginatedListTests
{
    [Fact(DisplayName = "Given items and metadata, when constructed, then properties are correctly initialized")]
    public void Given_ItemsAndMetadata_When_Constructed_Then_PropertiesInitialized()
    {
        // Arrange
        var items = new List<string> { "A", "B", "C" };
        const int totalCount = 10;
        const int pageNumber = 2;
        const int pageSize = 3; // 10 / 3 => 3.33 => 4 páginas

        // Act
        var paginatedList = new PaginatedList<string>(items, totalCount, pageNumber, pageSize);

        // Assert
        Assert.Equal(totalCount, paginatedList.TotalCount);
        Assert.Equal(pageSize, paginatedList.PageSize);
        Assert.Equal(pageNumber, paginatedList.CurrentPage);
        Assert.Equal(4, paginatedList.TotalPages); // Math.Ceiling(10/3) = 4

        // Itens internos (herda de List<T>)
        Assert.Equal(items.Count, paginatedList.Count);
        Assert.Equal(items, paginatedList.ToList());
    }

    [Theory(DisplayName = "Given CurrentPage and TotalPages, when checking HasPrevious and HasNext, then flags are correct")]
    [InlineData(1, 1, false, false)]
    [InlineData(1, 3, false, true)]
    [InlineData(2, 3, true,  true)]
    [InlineData(3, 3, true,  false)]
    public void Given_CurrentPageAndTotalPages_When_CheckingFlags_Then_HasPreviousAndHasNextAreCorrect(
        int currentPage, int totalPages, bool expectedHasPrevious, bool expectedHasNext)
    {
        // Arrange
        var items = new List<int> { 1, 2, 3 };
        var totalCount = totalPages * items.Count;
        var pageSize = items.Count;

        var paginatedList = new PaginatedList<int>(items, totalCount, currentPage, pageSize);

        Assert.Equal(totalPages, paginatedList.TotalPages);

        // Act + Assert
        Assert.Equal(expectedHasPrevious, paginatedList.HasPrevious);
        Assert.Equal(expectedHasNext, paginatedList.HasNext);
    }

    [Fact(DisplayName = "Given IQueryable source, when CreateAsync called, then returns PaginatedList with correct page items and metadata")]
    public async Task Given_IQueryableSource_When_CreateAsyncCalled_Then_ReturnsCorrectPaginatedList()
    {
        // Arrange
        var sourceList = Enumerable.Range(1, 50).ToList();
        var queryable = sourceList.AsQueryable();

        const int pageNumber = 2;
        const int pageSize = 10;

        // Act
        var paginatedList = await PaginatedList<int>.CreateAsync(queryable.ToTestAsyncEnumerable(), pageNumber, pageSize);

        // Assert
        Assert.Equal(sourceList.Count, paginatedList.TotalCount);
        Assert.Equal(pageSize, paginatedList.PageSize);
        Assert.Equal(pageNumber, paginatedList.CurrentPage);
        Assert.Equal(5, paginatedList.TotalPages); // 50 / 10

        // Itens da página 2: 11 a 20
        var expectedItems = sourceList.Skip(10).Take(10).ToList();
        Assert.Equal(expectedItems, paginatedList.ToList());
    }

    [Fact(DisplayName = "Given IQueryable source and first page, when CreateAsync called, then HasPrevious is false and HasNext is true")]
    public async Task Given_FirstPage_When_CreateAsyncCalled_Then_HasPreviousFalseAndHasNextTrue()
    {
        // Arrange
        var sourceList = Enumerable.Range(1, 25).ToList();
        var queryable = sourceList.AsQueryable();

        const int pageNumber = 1;
        const int pageSize = 10;

        // Act
        var paginatedList = await PaginatedList<int>.CreateAsync(queryable.ToTestAsyncEnumerable(), pageNumber, pageSize);

        // Assert
        Assert.False(paginatedList.HasPrevious);
        Assert.True(paginatedList.HasNext);
    }

    [Fact(DisplayName = "Given IQueryable source and last page, when CreateAsync called, then HasPrevious is true and HasNext is false")]
    public async Task Given_LastPage_When_CreateAsyncCalled_Then_HasPreviousTrueAndHasNextFalse()
    {
        // Arrange
        var sourceList = Enumerable.Range(1, 25).ToList();
        var queryable = sourceList.AsQueryable();

        const int pageNumber = 3; // 25 itens, pageSize=10 -> páginas 1,2,3
        const int pageSize = 10;

        // Act
        var paginatedList = await PaginatedList<int>.CreateAsync(queryable.ToTestAsyncEnumerable(), pageNumber, pageSize);

        // Assert
        Assert.True(paginatedList.HasPrevious);
        Assert.False(paginatedList.HasNext);

        // Última página: itens 21–25
        var expectedItems = sourceList.Skip(20).Take(10).ToList();
        Assert.Equal(expectedItems, paginatedList.ToList());
    }

    [Fact(DisplayName = "Given empty source, when CreateAsync called, then PaginatedList has zero items and zero pages")]
    public async Task Given_EmptySource_When_CreateAsyncCalled_Then_EmptyPaginatedList()
    {
        // Arrange
        var queryable = new List<int>().AsQueryable();

        const int pageNumber = 1;
        const int pageSize = 10;

        // Act
        var paginatedList = await PaginatedList<int>.CreateAsync(queryable.ToTestAsyncEnumerable(), pageNumber, pageSize);

        // Assert
        Assert.Empty(paginatedList);
        Assert.Equal(0, paginatedList.TotalCount);
        Assert.Equal(0, paginatedList.TotalPages);
        Assert.Equal(pageSize, paginatedList.PageSize);
        Assert.Equal(pageNumber, paginatedList.CurrentPage);
        Assert.False(paginatedList.HasPrevious);
        Assert.False(paginatedList.HasNext);
    }
}