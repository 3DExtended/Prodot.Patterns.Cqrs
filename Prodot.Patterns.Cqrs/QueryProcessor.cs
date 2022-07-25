using System.Diagnostics;
using System.Reflection;

namespace Prodot.Patterns.Cqrs;

public sealed class QueryProcessor : IQueryProcessor
{
    private readonly MethodInfo _factoryMethodInfo;
    private readonly IQueryHandlerFactory _queryHandlerFactory;
    private readonly IQueryHandlerRegistry _queryHandlerRegistry;

    public QueryProcessor(IQueryHandlerRegistry queryHandlerRegistry, IQueryHandlerFactory queryHandlerFactory)
    {
        _queryHandlerRegistry = queryHandlerRegistry;
        _queryHandlerFactory = queryHandlerFactory;
        _factoryMethodInfo = _queryHandlerFactory
            .GetType()
            .GetMethod(nameof(IQueryHandlerFactory.CreateQueryHandler)) ?? default!;

        if (_factoryMethodInfo is null)
        {
            throw new ArgumentException(
                "Could not find CreateQueryHandler method on QueryHandlerFactory. Did you forget to implement it as a public method?");
        }
    }

    [DebuggerStepThrough]
    public async Task<Option<TResult>> RunQueryAsync<TQuery, TResult>(IQuery<TResult, TQuery> query,
        CancellationToken cancellationToken) where TQuery : IQuery<TResult, TQuery>
    {
        ValidateQuery(query);
        var handler = GetHandlerHierarchyForQuery<TQuery, TResult>();
        try
        {
            return await handler.RunQueryAsync((TQuery)query, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            do
            {
                var successor = handler.Successor;
                _queryHandlerFactory.ReturnQueryHandler(handler);
                handler = successor;
            }
            while (handler != null);
        }
    }

    private static void ValidateQuery<TQuery, TResult>(IQuery<TResult, TQuery> query)
        where TQuery : IQuery<TResult, TQuery>
    {
        foreach (var property in typeof(TQuery).GetProperties())
        {
            var pipelineRuntimeValueAttributes = property.GetCustomAttributes(typeof(PipelineRuntimeValueAttribute))
                .Cast<PipelineRuntimeValueAttribute>()
                .ToList();
            if (pipelineRuntimeValueAttributes.Count > 0)
            {
                // since this is a runtime value, it must have it's default value on start
                var propertyTypeDefaultValue = property.PropertyType.IsValueType
                    ? Activator.CreateInstance(property.PropertyType)
                    : null;
                var propertyHasDefaultValue = Equals(property.GetValue(query), propertyTypeDefaultValue);
                if (!propertyHasDefaultValue)
                {
                    throw new ArgumentException(
                        "Properties marked with [PipelineRuntimeValueAttribute] must have default value on execution start.",
                        property.Name);
                }
            }
            else
            {
                // if not marked as a runtime value, a property may not be null (use Option<T> for optional properties)
                var propertyIsNull = Equals(property.GetValue(query), null);
                if (propertyIsNull)
                {
                    throw new ArgumentException(
                        "Properties not marked with [PipelineRuntimeValueAttribute] must not be null on execution start. For optional values, use Option<T>.",
                        property.Name);
                }
            }
        }
    }

    [DebuggerStepThrough]
    private IQueryHandler<TQuery, TResult> GetHandlerHierarchyForQuery<TQuery, TResult>()
        where TQuery : IQuery<TResult, TQuery>
    {
        var pipeline = _queryHandlerRegistry.GetPipelineForQuery(typeof(TQuery), _queryHandlerFactory);
        var firstHandler = GetRequestHandler<TQuery, TResult>(pipeline.Parts[0]);
        var currentHandler = firstHandler;

        // make sure to propagate query handler configuration if set
        if (pipeline.Parts[0].HandlerConfiguration != null)
        {
            var configurationProperty = currentHandler.GetType()
                .GetProperty(nameof(IConfigurableQueryHandler<int>.Configuration));
            if (configurationProperty is null)
            {
                throw new ArgumentException(
                    $"Found configuration value in pipeline but could not find configuration property on query handler {currentHandler.GetType().FullName}");
            }

            configurationProperty!.SetValue(currentHandler, pipeline.Parts[0].HandlerConfiguration);
        }

        foreach (var part in pipeline.Parts.Skip(1))
        {
            var nextHandler = GetRequestHandler<TQuery, TResult>(part);
            currentHandler.Successor = nextHandler;

            // make sure to propagate query handler configuration if set
            if (part.HandlerConfiguration != null)
            {
                var configurationProperty = nextHandler.GetType()
                    .GetProperty(nameof(IConfigurableQueryHandler<int>.Configuration));
                if (configurationProperty is null)
                {
                    throw new ArgumentException(
                        $"Found configuration value in pipeline but could not find configuration property on query handler {currentHandler.GetType().FullName}");
                }

                configurationProperty!.SetValue(nextHandler, part.HandlerConfiguration);
            }

            currentHandler = nextHandler;
        }

        // last handler gets noop as successor, to ensure that it _can_ call the successor
        currentHandler.Successor = new NoopQueryHandler<TQuery, TResult>();

        return firstHandler;
    }

    [DebuggerStepThrough]
    private IQueryHandler<TQuery, TResult> GetRequestHandler<TQuery, TResult>(PipelinePart descriptor)
        where TQuery : IQuery<TResult, TQuery>
    {
        var method = _factoryMethodInfo.MakeGenericMethod(descriptor.HandlerType, typeof(TQuery), typeof(TResult));
        return (IQueryHandler<TQuery, TResult>?)method.Invoke(_queryHandlerFactory, new object[0]) ?? default!;
    }
}
