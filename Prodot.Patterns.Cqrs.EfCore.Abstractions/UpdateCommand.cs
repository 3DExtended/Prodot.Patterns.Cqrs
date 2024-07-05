namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class UpdateCommand<TModel, TIdentifier, TIdentifierValue, TSelf> : Command<TSelf>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TSelf : UpdateCommand<TModel, TIdentifier, TIdentifierValue, TSelf>
{
    public TModel UpdatedModel { get; init; } = default!;
}
