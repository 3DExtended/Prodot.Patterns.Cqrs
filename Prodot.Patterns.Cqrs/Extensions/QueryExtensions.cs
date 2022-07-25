namespace Prodot.Patterns.Cqrs;

public static class QueryExtensions
{
    /// <summary>
    /// Executes this query with the given query processor.
    /// </summary>
    public static Task<Option<TResult>> RunAsync<TQuery, TResult>(this IQuery<TResult, TQuery> query, IQueryProcessor queryProcessor, CancellationToken cancellationToken)
        where TQuery : IQuery<TResult, TQuery>
        => queryProcessor.RunQueryAsync(query, cancellationToken);

    /// <summary>
    /// Executes this query with the given query processor.
    /// If the result is <see cref="Option.None"/>, a <see cref="PipelineExecutionException"/> is thrown.
    /// Otherwise, the unwrapped result is returned.
    /// </summary>
    public static async Task<TResult> RunAsyncWithDefaultExceptionIfNone<TQuery, TResult>(
        this IQuery<TResult, TQuery> query,
        IQueryProcessor queryProcessor,
        CancellationToken cancellationToken)
        where TQuery : IQuery<TResult, TQuery>
    {
        var result = await RunAsync(query, queryProcessor, cancellationToken).ConfigureAwait(false);
        if (result.IsNone)
        {
            throw new PipelineExecutionException(query);
        }

        return result.Get();
    }

    /// <summary>
    /// If the result of the pipeline execution is none, the exception created by the exceptionFactory is thrown.
    /// </summary>
    public static async Task<TResult> WithExceptionIfNone<TResult>(this Task<Option<TResult>> queryProcessorTask, Func<Exception> exceptionFactory)
    {
        var result = await queryProcessorTask.ConfigureAwait(false);
        if (result.IsNone)
        {
            throw exceptionFactory();
        }

        return result.Get();
    }
}
