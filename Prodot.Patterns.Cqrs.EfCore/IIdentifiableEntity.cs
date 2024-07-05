namespace Prodot.Patterns.Cqrs.EfCore;

public interface IIdentifiableEntity<TIdentifierValue>
{
    TIdentifierValue Id { get; }
}
