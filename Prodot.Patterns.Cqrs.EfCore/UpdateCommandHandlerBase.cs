namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class UpdateCommandHandlerBase<
    TQuery,
    TModel,
    TIdentifier,
    TIdentifierValue,
    TContext,
    TEntity
> : IQueryHandler<TQuery, Unit>
    where TQuery : UpdateCommand<TModel, TIdentifier, TIdentifierValue, TQuery>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TContext : DbContext
    where TEntity : class, IIdentifiableEntity<TIdentifierValue>
{
    private readonly IDbContextFactory<TContext> _contextFactory;
    private readonly IMapper _mapper;

    protected UpdateCommandHandlerBase(IMapper mapper, IDbContextFactory<TContext> contextFactory)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
    }

    public IQueryHandler<TQuery, Unit> Successor { get; set; } = default!;

    public async Task<Option<Unit>> RunQueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        using (
            var context = await _contextFactory
                .CreateDbContextAsync(cancellationToken)
                .ConfigureAwait(false)
        )
        {
            var entity = _mapper.Map<TEntity>(query.UpdatedModel);

            var existingEntity = await context
                .Set<TEntity>()
                .FirstOrDefaultAsync(
                    e => e.Id!.Equals(query.UpdatedModel.Id.Value),
                    cancellationToken
                )
                .ConfigureAwait(false);

            if (existingEntity == null)
            {
                return Option.None;
            }

            _mapper.Map(entity, existingEntity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        return Unit.Value;
    }
}
