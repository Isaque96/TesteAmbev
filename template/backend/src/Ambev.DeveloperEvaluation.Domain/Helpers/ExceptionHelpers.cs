namespace Ambev.DeveloperEvaluation.Domain.Helpers;

public static class ExceptionHelpers
{
    public static IEnumerable<Exception> GetAllExceptions(this Exception exception)
    {
        var ex = exception;
        while (ex != null)
        {
            yield return ex;
            ex = ex.InnerException;
        }
    }
}