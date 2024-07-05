namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Base class for implementing configurable query handlers (used as a convenience).
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public abstract class ConfigurableQueryHandler<TQuery, TResult, TConfiguration>
    : IQueryHandler<TQuery, TResult>,
        IConfigurableQueryHandler<TConfiguration>
    where TQuery : IQuery<TResult, TQuery>
{
    public TConfiguration Configuration { protected get; set; } = default!;

    public IQueryHandler<TQuery, TResult> Successor { get; set; } = default!;

    public abstract Task<Option<TResult>> RunQueryAsync(
        TQuery query,
        CancellationToken cancellationToken
    );
}
