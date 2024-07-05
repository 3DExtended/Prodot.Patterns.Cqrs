namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class CountQuery<TModel, TIdentifier, TIdentifierValue, TSelf> : IQuery<int, TSelf>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TSelf : CountQuery<TModel, TIdentifier, TIdentifierValue, TSelf> { }
