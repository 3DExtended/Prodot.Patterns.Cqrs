namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class SingleModelQueryHandlerBase<TQuery, TModel, TIdentifier, TIdentifierValue, TContext, TEntity> : IQueryHandler<TQuery, TModel>
    where TQuery : SingleModelQuery<TModel, TIdentifier, TIdentifierValue, TQuery>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TContext : DbContext
    where TEntity : class, IIdentifiableEntity<TIdentifierValue>
{
    private readonly IDbContextFactory<TContext> _contextFactory;
    private readonly IMapper _mapper;

    protected SingleModelQueryHandlerBase(IMapper mapper, IDbContextFactory<TContext> contextFactory)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
    }

    public IQueryHandler<TQuery, TModel> Successor { get; set; } = default!;

    public async Task<Option<TModel>> RunQueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        using (var context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false))
        {
            var databaseQuery = AddIncludes(context.Set<TEntity>().AsNoTracking());

            var entity = await databaseQuery
               .FirstOrDefaultAsync(cp => cp.Id!.Equals(query.Id.Value), cancellationToken)
               .ConfigureAwait(false);

            return entity == null ? Option.None : Option.From(_mapper.Map<TModel>(entity));
        }
    }

    /// <summary>
    /// Override this method to add '.Include(...)' calls for retrieving the entities.
    /// </summary>
    protected virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> queryable) => queryable;
}
