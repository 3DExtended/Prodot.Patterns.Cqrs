namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class CountQueryHandlerBase<TQuery, TModel, TIdentifier, TIdentifierValue, TContext, TEntity> : IQueryHandler<TQuery, int>
    where TQuery : CountQuery<TModel, TIdentifier, TIdentifierValue, TQuery>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TContext : DbContext
    where TEntity : class, IIdentifiableEntity<TIdentifierValue>
{
    private readonly IDbContextFactory<TContext> _contextFactory;

    protected CountQueryHandlerBase(IDbContextFactory<TContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public IQueryHandler<TQuery, int> Successor { get; set; } = default!;

    public async Task<Option<int>> RunQueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        using (var context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false))
        {
            var efquery = PrepareQuery(context.Set<TEntity>());
            return await efquery.CountAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Override this method to add any query modifications (e.g. .Where(...)) before CountAsync is called.
    /// </summary>
    protected virtual IQueryable<TEntity> PrepareQuery(IQueryable<TEntity> efquery) => efquery;
}
