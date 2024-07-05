namespace Prodot.Patterns.Cqrs;

public interface IQueryProcessor
{
    Task<Option<TResult>> RunQueryAsync<TQuery, TResult>(
        IQuery<TResult, TQuery> query,
        CancellationToken cancellationToken
    )
        where TQuery : IQuery<TResult, TQuery>;
}
