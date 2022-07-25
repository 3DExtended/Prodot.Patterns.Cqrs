namespace Prodot.Patterns.Cqrs;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult, TQuery>
{
    IQueryHandler<TQuery, TResult> Successor { get; set; }

    Task<Option<TResult>> RunQueryAsync(TQuery query, CancellationToken cancellationToken);
}
