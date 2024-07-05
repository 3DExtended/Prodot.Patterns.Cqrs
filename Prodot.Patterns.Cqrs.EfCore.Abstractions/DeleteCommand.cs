namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class DeleteCommand<TModel, TIdentifier, TIdentifierValue, TSelf> : Command<TSelf>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TSelf : DeleteCommand<TModel, TIdentifier, TIdentifierValue, TSelf>
{
    public TIdentifier Id { get; init; } = default!;
}
