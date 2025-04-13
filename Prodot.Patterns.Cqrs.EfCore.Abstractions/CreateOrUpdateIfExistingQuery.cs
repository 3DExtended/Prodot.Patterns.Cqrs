namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class CreateOrUpdateIfExistingQuery<TModel, TIdentifier, TIdentifierValue, TSelf>
    : IQuery<TIdentifier, TSelf>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TSelf : CreateOrUpdateIfExistingQuery<TModel, TIdentifier, TIdentifierValue, TSelf>
{
    public TModel ModelToCreate { get; init; } = default!;
}
