namespace Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection.Tests.TestHelpers;

public class GenericQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult, TQuery>
{
    public IQueryHandler<TQuery, TResult> Successor { get; set; } = default!;

    public Task<Option<TResult>> RunQueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(Option.From<TResult>(default(TResult)));
    }
}
