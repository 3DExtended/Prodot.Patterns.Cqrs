using Microsoft.Extensions.DependencyInjection;

namespace Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProdotPatternsCqrs(
        this IServiceCollection serviceCollection,
        Action<ProdotPatternsCqrsOptions> configure
    )
    {
        var registryBuilder = QueryHandlerRegistry.Builder();

        // run configuration
        var options = new ProdotPatternsCqrsOptions(registryBuilder);
        configure(options);

        // register pipeline registry
        serviceCollection.AddSingleton(options.QueryHandlerRegistryBuilder.Build());

        // register factory
        serviceCollection.AddScoped<
            IQueryHandlerFactory,
            ServiceProviderBasedQueryHandlerFactory
        >();

        // register queryprocessor
        serviceCollection.AddScoped<IQueryProcessor, QueryProcessor>();

        // Register Query handlers
        var queryHandlerTypes = options
            .AssembliesToLoadQueryHandlersFrom.SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && IsQueryHandlerType(t))
            .ToList();

        foreach (var handlerType in queryHandlerTypes)
        {
            // register query handler type as itself
            serviceCollection.AddTransient(handlerType);

            if (handlerType.IsGenericTypeDefinition)
            {
                // open generic query handlers may be registered but only used by explicitly specifying them in a pipeline
                continue;
            }

            foreach (var queryHandlerInterface in GetQueryHandlerInterfaces(handlerType))
            {
                // register query handler type by each of its query handler interfaces
                serviceCollection.AddTransient(queryHandlerInterface, handlerType);
            }
        }

        return serviceCollection;
    }

    private static IReadOnlyList<Type> GetQueryHandlerInterfaces(Type type) =>
        type.GetInterfaces()
            .Where(t =>
                t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(IQueryHandler<,>))
            )
            .ToList();

    private static bool IsQueryHandlerType(Type type) =>
        type.GetInterfaces()
            .Where(t => t.IsGenericType)
            .Select(t => t.GetGenericTypeDefinition())
            .Any(t => t.Equals(typeof(IQueryHandler<,>)));
}
