namespace Prodot.Patterns.Cqrs.EfCore;

/// <summary>
/// Base class for queries that retrieve a list of models.
/// </summary>
public abstract class ListOfModelQuery<TModel, TIdentifier, TIdentifierValue, TSelf>
    : IQuery<IReadOnlyList<TModel>, TSelf>
    where TModel : ModelBase<TIdentifier, TIdentifierValue>
    where TIdentifier : Identifier<TIdentifierValue, TIdentifier>, new()
    where TSelf : ListOfModelQuery<TModel, TIdentifier, TIdentifierValue, TSelf>
{
    /// <summary>
    /// If true, the result will be returned as successful even if not all specified Ids could be retrieved.
    /// Otherwise, <see cref="Option.None"/> is returned in that case.
    /// </summary>
    public bool AllowPartialResultSet { get; init; }

    /// <summary>
    /// A list of Ids to retrieve or <see cref="Option.None"/> to retrieve all.
    /// </summary>
    public Option<IReadOnlyList<TIdentifier>> Ids { get; init; } = default!;
}
