namespace Prodot.Patterns.Cqrs.Tests.TestHelpers;

internal class CallbackQueryHandlerFactory : IQueryHandlerFactory
{
    private readonly Action? _postSuccessorCallback;
    private readonly Action<bool>? _postSuccessorCallbackWithConfiguration;
    private readonly Action? _preSuccessorCallback;
    private readonly Action<bool>? _preSuccessorCallbackWithConfiguration;

    public CallbackQueryHandlerFactory(Action preSuccessorCallback, Action postSuccessorCallback)
    {
        _preSuccessorCallback = preSuccessorCallback;
        _postSuccessorCallback = postSuccessorCallback;
    }

    public CallbackQueryHandlerFactory(
        Action<bool> preSuccessorCallbackWithConfiguration,
        Action<bool> postSuccessorCallbackWithConfiguration
    )
    {
        _preSuccessorCallbackWithConfiguration = preSuccessorCallbackWithConfiguration;
        _postSuccessorCallbackWithConfiguration = postSuccessorCallbackWithConfiguration;
    }

    public IQueryHandler<TQuery, TResult> CreateQueryHandler<THandlerType, TQuery, TResult>()
        where THandlerType : class, IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult, TQuery>
    {
        if (_preSuccessorCallbackWithConfiguration == null)
        {
            return new CallbackQueryHandler<TQuery, TResult>
            {
                PreSuccessorCallback = _preSuccessorCallback ?? default!,
                PostSuccessorCallback = _postSuccessorCallback ?? default!
            };
        }

        return new ConfigurableCallbackQueryHandler<TQuery, TResult>
        {
            PreSuccessorCallbackWithConfiguration = _preSuccessorCallbackWithConfiguration,
            PostSuccessorCallbackWithConfiguration = _postSuccessorCallbackWithConfiguration
        };
    }

    public void ReturnQueryHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler)
        where TQuery : IQuery<TResult, TQuery> { }
}
