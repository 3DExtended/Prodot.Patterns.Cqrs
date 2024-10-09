namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
{
    public TIdentifier Id { get; set; } =
        Identifier<TIdentifierValue, TIdentifier>.From((TIdentifierValue)default!);
}
