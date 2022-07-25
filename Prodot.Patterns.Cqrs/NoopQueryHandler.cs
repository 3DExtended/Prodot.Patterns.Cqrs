namespace Prodot.Patterns.Cqrs;

internal sealed class NoopQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult, TQuery>
{
    public IQueryHandler<TQuery, TResult> Successor { get; set; } = default!;

    public Task<Option<TResult>> RunQueryAsync(TQuery query, CancellationToken cancellationToken)
        => Task.FromResult(Option<TResult>.None);
}
