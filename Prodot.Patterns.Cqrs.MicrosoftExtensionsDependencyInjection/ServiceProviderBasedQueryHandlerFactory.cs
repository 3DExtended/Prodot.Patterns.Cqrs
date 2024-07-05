using Microsoft.Extensions.DependencyInjection;

namespace Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection;

internal class ServiceProviderBasedQueryHandlerFactory : IQueryHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderBasedQueryHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IQueryHandler<TQuery, TResult> CreateQueryHandler<THandlerType, TQuery, TResult>()
        where THandlerType : class, IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult, TQuery> =>
        _serviceProvider.GetRequiredService<THandlerType>();

    public void ReturnQueryHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler)
        where TQuery : IQuery<TResult, TQuery>
    {
        if (handler is IDisposable d)
        {
            d.Dispose();
        }
    }
}
