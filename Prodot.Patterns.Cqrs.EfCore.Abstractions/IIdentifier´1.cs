namespace Prodot.Patterns.Cqrs.EfCore.Abstractions;

public interface IIdentifier<TIdentifierValue>
{
    TIdentifierValue Value { get; }
}
