namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Convenience class for implementing command handlers (query handlers with return type Unit).
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
public abstract class CommandHandler<TQuery> : IQueryHandler<TQuery, Unit> where TQuery : IQuery<Unit, TQuery>
{
    public IQueryHandler<TQuery, Unit> Successor { get; set; } = default!;

    public abstract Task<Option<Unit>> RunQueryAsync(TQuery query, CancellationToken cancellationToken);
}
