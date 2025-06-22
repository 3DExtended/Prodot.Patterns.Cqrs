using System.Linq.Expressions;

namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class CreateOrUpdateIfExistingQueryHandlerBase<
    TQuery,
    TModel,
    TIdentifier,
    TIdentifierValue,
    TContext,
    TEntity
> : IQueryHandler<TQuery, TIdentifier>
    where TQuery : CreateOrUpdateIfExistingQuery<TModel, TIdentifier, TIdentifierValue, TQuery>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TContext : DbContext
    where TEntity : class, IIdentifiableEntity<TIdentifierValue>
{
    private readonly IDbContextFactory<TContext> _contextFactory;
    private readonly IMapper _mapper;

    protected CreateOrUpdateIfExistingQueryHandlerBase(
        IMapper mapper,
        IDbContextFactory<TContext> contextFactory
    )
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
    }

    public IQueryHandler<TQuery, TIdentifier> Successor { get; set; } = default!;

    public async Task<Option<TIdentifier>> RunQueryAsync(
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
            var preparedModel = await PrepareModelAsync(
                    query.ModelToCreate,
                    context,
                    cancellationToken
                )
                .ConfigureAwait(false);
            if (preparedModel.IsNone)
            {
                return Option.None;
            }

            var entity = _mapper.Map<TEntity>(preparedModel.Get());

            var existingEntity = await context
                .Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(GetPossiblyExistingEntity(query), cancellationToken)
                .ConfigureAwait(false);

            if (existingEntity != null)
            {
                _mapper.Map(entity, existingEntity);
                context.Update(existingEntity);
            }
            else
            {
                await context
                    .Set<TEntity>()
                    .AddAsync(entity, cancellationToken)
                    .ConfigureAwait(false);
            }

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var id = Identifier<TIdentifierValue, TIdentifier>.From(entity.Id);
            await AfterCreationAsync(query, id, cancellationToken).ConfigureAwait(false);
            return id;
        }
    }

    protected abstract Task<Option<TModel>> PrepareModelAsync(
        TModel modelToCreate,
        TContext context,
        CancellationToken cancellationToken
    );

    protected abstract Expression<Func<TEntity, bool>> GetPossiblyExistingEntity(TQuery query);

    protected virtual Task AfterCreationAsync(
        TQuery query,
        TIdentifier id,
        CancellationToken cancellationToken
    ) => Task.CompletedTask;
}
