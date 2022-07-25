using Microsoft.Extensions.DependencyInjection;

namespace Prodot.Patterns.Cqrs.Tests.TestHelpers;

public class DependencyInjectionQueryHandlerFactory : IQueryHandlerFactory
{
    private IServiceProvider? _serviceProvider;

    public IServiceProvider ServiceProvider
    {
        get
        {
            if (_serviceProvider is null)
            {
                _serviceProvider = Services.BuildServiceProvider();
            }

            return _serviceProvider;
        }
    }

    public IServiceCollection Services { get; } = new ServiceCollection();

    public IQueryHandler<TQuery, TResult> CreateQueryHandler<THandlerType, TQuery, TResult>()
        where THandlerType : class, IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult, TQuery>
        => ServiceProvider.GetRequiredService<THandlerType>();

    public void ReturnQueryHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult, TQuery>
    {
    }
}
