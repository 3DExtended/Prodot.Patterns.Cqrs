namespace Prodot.Patterns.Cqrs.Extensions;

public static class QueryProcessorExtensions
{
    /// <summary>
    /// Runs an untyped query.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="queryProcessor">The query processor.</param>
    /// <param name="query">The query to run. Must implement <see cref="IQuery{TResult, TSelf}"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the query execution.</returns>
    /// <exception cref="ArgumentException">If the query does not implement <see cref="IQuery{TResult, TSelf}"/>.</exception>
    public static Task<Option<TResult>> RunDynamicAsync<TResult>(
        this IQueryProcessor queryProcessor,
        object query,
        CancellationToken cancellationToken
    )
    {
        var queryType = query.GetType();

        if (!queryType.IsGenericType || queryType.GetGenericTypeDefinition() != typeof(IQuery<,>))
        {
            throw new ArgumentException(
                "Provided query does not implement interface IQuery<,>.",
                nameof(query)
            );
        }

        // TODO iterate through interface hierarchies recursively
        var returnType = queryType
            .GetInterfaces()
            .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<,>))
            .GetGenericArguments()[0];
        var runAsyncMethod = queryProcessor
            .GetType()
            .GetMethod(nameof(queryProcessor.RunQueryAsync));
        var runAsyncMethodGeneric = runAsyncMethod!.MakeGenericMethod(queryType, returnType);

        return (
                (Task<Option<TResult>>?)
                    runAsyncMethodGeneric.Invoke(
                        queryProcessor,
                        new object[] { query, cancellationToken }
                    )
            ) ?? default!;
    }
}
