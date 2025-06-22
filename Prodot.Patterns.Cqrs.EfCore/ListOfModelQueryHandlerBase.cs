namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class ListOfModelQueryHandlerBase<
    TQuery,
    TModel,
    TIdentifier,
    TIdentifierValue,
    TContext,
    TEntity
> : IQueryHandler<TQuery, IReadOnlyList<TModel>>
    where TQuery : ListOfModelQuery<TModel, TIdentifier, TIdentifierValue, TQuery>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TContext : DbContext
    where TEntity : class, IIdentifiableEntity<TIdentifierValue>
{
    private readonly IDbContextFactory<TContext> _contextFactory;
    private readonly IMapper _mapper;

    protected ListOfModelQueryHandlerBase(
        IMapper mapper,
        IDbContextFactory<TContext> contextFactory
    )
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
    }

    public IQueryHandler<TQuery, IReadOnlyList<TModel>> Successor { get; set; } = default!;

    public async Task<Option<IReadOnlyList<TModel>>> RunQueryAsync(
        TQuery query,
        CancellationToken cancellationToken
    )
    {
        using (
            var context = await _contextFactory
                .CreateDbContextAsync(cancellationToken)
                .ConfigureAwait(false)
        )
        {
            var databaseQuery = AddWhere(AddIncludes(context.Set<TEntity>().AsNoTracking()), query);

            if (query.Ids.IsSome)
            {
                var ids = query.Ids.Get().Select(id => id.Value).Distinct().ToList();
                databaseQuery = databaseQuery.Where(e => ids.Contains(e.Id));
            }

            var entities = await databaseQuery.ToListAsync(cancellationToken).ConfigureAwait(false);

            if (
                !query.AllowPartialResultSet
                && query.Ids.IsSome
                && query.Ids.Get().Distinct().Count() != entities.Count
            )
            {
                // not all requested IDs were found
                return Option.None;
            }

            return entities.ConvertAll(_mapper.Map<TModel>);
        }
    }

    /// <summary>
    /// Override this method to add '.Include(...)' calls for retrieving the entities.
    /// </summary>
    protected virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> queryable) => queryable;

    /// <summary>
    /// Override this method to add '.Where(...)' calls for retrieving the entities.
    /// </summary>
    protected virtual IQueryable<TEntity> AddWhere(IQueryable<TEntity> queryable, TQuery query) =>
        queryable;
}
