namespace Prodot.Patterns.Cqrs.EfCore;

public abstract class SingleModelBySelectorQuery<TModel, TIdentifier, TIdentifierValue, TSelf>
    : IQuery<TModel, TSelf>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TSelf : SingleModelBySelectorQuery<TModel, TIdentifier, TIdentifierValue, TSelf> { }
