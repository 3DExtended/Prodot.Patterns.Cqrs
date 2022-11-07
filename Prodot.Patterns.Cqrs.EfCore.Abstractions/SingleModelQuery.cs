namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class SingleModelQuery<TModel, TIdentifier, TIdentifierValue, TSelf> : IQuery<TModel, TSelf>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TSelf : SingleModelQuery<TModel, TIdentifier, TIdentifierValue, TSelf>
{
    public TIdentifier ModelId { get; set; } = default!;
}
