using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Ambev.DeveloperEvaluation.Unit.Helpers;

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

    public TestAsyncEnumerable(Expression expression) : base(expression) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

public class TestAsyncEnumerator<T>(IEnumerator<T> inner) : IAsyncEnumerator<T>
{
    public T Current => inner.Current;

    public ValueTask DisposeAsync()
    {
        inner.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(inner.MoveNext());
}

public class TestAsyncQueryProvider<TEntity>(IQueryProvider inner) : IAsyncQueryProvider
{
    public IQueryable CreateQuery(Expression expression)
        => new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new TestAsyncEnumerable<TElement>(expression);

    public object? Execute(Expression expression) => inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
        => inner.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var expectedResultType = typeof(TResult);

        // Se for Task<T>, precisamos invocar dinamicamente e retornar a Task
        if (expectedResultType.IsGenericType &&
            expectedResultType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var resultType = expectedResultType.GenericTypeArguments[0];

            var lambda = Expression.Lambda(expression);
            var compiled = lambda.Compile();
            var result = compiled.DynamicInvoke();

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(resultType)
                .Invoke(null, [result])!;
        }

        // Se não for Task<>, provavelmente é um Task, tipo Task.CompletedTask
        var execResult = Execute<TResult>(expression);
        return execResult;
    }
}

public static class AsyncQueryableExtensions
{
    public static TestAsyncEnumerable<T> ToTestAsyncEnumerable<T>(this IEnumerable<T> source)
        => new(source);
}
