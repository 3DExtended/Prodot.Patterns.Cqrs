namespace Prodot.Patterns.Cqrs.Tests.TestHelpers;

internal class CallbackQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult, TQuery>
{
    public Action PostSuccessorCallback { get; set; } = () => { };

    public Action PreSuccessorCallback { get; set; } = () => { };

    public IQueryHandler<TQuery, TResult> Successor { get; set; } = default!;

    public async Task<Option<TResult>> RunQueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        PreSuccessorCallback?.Invoke();
        var result = await Successor.RunQueryAsync(query, cancellationToken).ConfigureAwait(false);
        PostSuccessorCallback?.Invoke();
        return result;
    }
}
