namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Responsible for instantiating and handling the lifecycle of handler instances.
/// </summary>
public interface IQueryHandlerFactory
{
    /// <summary>
    /// Returns a handler instance of the specified type, potantially resolved from a DI-container.
    /// </summary>
    IQueryHandler<TQuery, TResult> CreateQueryHandler<THandlerType, TQuery, TResult>()
        where THandlerType : class, IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult, TQuery>;

    /// <summary>
    /// Returns a used handler instance for further processing, e.g. to support DI-container scoped lifestyles.
    /// </summary>
    void ReturnQueryHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler)
        where TQuery : IQuery<TResult, TQuery>;
}
