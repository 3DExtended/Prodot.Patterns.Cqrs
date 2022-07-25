namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Convenience class for implementing query handlers.
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult, TQuery>
{
    public IQueryHandler<TQuery, TResult> Successor { get; set; } = default!;

    public abstract Task<Option<TResult>> RunQueryAsync(TQuery query, CancellationToken cancellationToken);
}
