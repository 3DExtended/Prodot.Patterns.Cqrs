namespace Prodot.Patterns.Cqrs.Tests.TestHelpers;

public class ConfigurableCallbackQueryHandler<TQuery, TResult>
    : IQueryHandler<TQuery, TResult>,
        IConfigurableQueryHandler<bool>
    where TQuery : IQuery<TResult, TQuery>
{
    public bool Configuration { get; set; }

    public Action<bool>? PostSuccessorCallbackWithConfiguration { get; set; }

    public Action<bool>? PreSuccessorCallbackWithConfiguration { get; set; }

    public IQueryHandler<TQuery, TResult> Successor { get; set; } = default!;

    public async Task<Option<TResult>> RunQueryAsync(
        TQuery query,
        CancellationToken cancellationToken
    )
    {
        PreSuccessorCallbackWithConfiguration?.Invoke(Configuration);
        var result = await Successor.RunQueryAsync(query, cancellationToken).ConfigureAwait(false);
        PostSuccessorCallbackWithConfiguration?.Invoke(Configuration);
        return result;
    }
}
