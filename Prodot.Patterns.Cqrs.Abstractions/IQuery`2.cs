namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Interface for queries.
/// </summary>
/// <typeparam name="TResult">The result of the query.</typeparam>
/// <typeparam name="TSelf">The type of the query itself, e.g. <code>public class AQuery : IQuery&lt;int, AQuery&gt;</code></typeparam>
public interface IQuery<TResult, TSelf>
    where TSelf : IQuery<TResult, TSelf> { }
