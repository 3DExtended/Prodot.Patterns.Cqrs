namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class CreateQuery<TModel, TIdentifier, TIdentifierValue, TSelf>
    : IQuery<TIdentifier, TSelf>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TSelf : CreateQuery<TModel, TIdentifier, TIdentifierValue, TSelf>
{
    public TModel ModelToCreate { get; init; } = default!;
}
