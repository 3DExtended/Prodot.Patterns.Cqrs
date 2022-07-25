using System.Reflection;

using static Prodot.Patterns.Cqrs.QueryHandlerRegistry;

namespace Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection;

public class ProdotPatternsCqrsOptions
{
    private readonly List<Assembly> _assembliesToLoadQueryHandlersFrom = new();
    private readonly IQueryHandlerRegistryBuilder _queryHandlerRegistryBuilder;

    internal ProdotPatternsCqrsOptions(IQueryHandlerRegistryBuilder queryHandlerRegistryBuilder)
    {
        _queryHandlerRegistryBuilder = queryHandlerRegistryBuilder;
    }

    public IReadOnlyList<Assembly> AssembliesToLoadQueryHandlersFrom => _assembliesToLoadQueryHandlersFrom;

    public IQueryHandlerRegistryBuilder QueryHandlerRegistryBuilder => _queryHandlerRegistryBuilder;

    public ProdotPatternsCqrsOptions WithQueryHandlerPipelineConfiguration(Action<IQueryHandlerRegistryBuilder> configureRegistry)
    {
        configureRegistry(QueryHandlerRegistryBuilder);
        return this;
    }

    public ProdotPatternsCqrsOptions WithQueryHandlersFrom(params Assembly[] assemblies)
    {
        _assembliesToLoadQueryHandlersFrom.AddRange(assemblies);
        return this;
    }
}
