using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace Prodot.Patterns.Cqrs;

public sealed class QueryHandlerRegistry : IQueryHandlerRegistry
{
    private readonly ConcurrentDictionary<Type, Pipeline> _pipelineByQueryTypeDictionary = new();

    private QueryHandlerRegistry(bool isPipelineAutoRegistrationEnabled = false)
    {
        IsPipelineAutoRegistrationEnabled = isPipelineAutoRegistrationEnabled;
    }

    public interface IQueryHandlerRegistryBuilder
    {
        /// <summary>
        /// Configures the builder by registering pipelines from profiles loaded from the given assemblies. This is the recommended production approach.
        /// </summary>
        IQueryHandlerRegistryBuilder AddProfiles(params Assembly[] assembliesToSearchProfilesIn);

        /// <summary>
        /// Configures the builder by registering pipelines directly within a callback. Mainly used for testing.
        /// </summary>
        IQueryHandlerRegistryBuilder AddRegisterCallback(Action<Action<Pipeline>> registerCallback);

        /// <summary>
        /// return the created and configured registry.
        /// </summary>
        IQueryHandlerRegistry Build();

        /// <summary>
        /// Enables pipeline auto registration.
        /// In case of a missing pipeline, the registry will try to automatically create and register a pipeline on-the-fly if no pipeline for a query is registered.
        /// For that, it will search all registered non-generic query handlers and if only one is found for the query, that one will be used for the pipeline.
        ///
        /// Note: For auto-registration to work, your <see cref="IQueryHandlerFactory"/> implementation must support creating query handlers by concrete type as well as
        /// by interface (e.g. CreateQueryHandler&lt;AQueryHandler, AQuery, Unit&gt;() must work as well as CreateQueryHandler&lt;IQueryHandler&lt;AQuery, Unit&gt;, AQuery, Unit&gt;()).
        /// If creation by interface is ambiguous, it should return null or throw an exception.
        /// </summary>
        IQueryHandlerRegistryBuilder WithPipelineAutoRegistration();
    }

    /// <summary>
    /// If true, the registry will try to automatically create a pipeline on-the-fly if no pipeline for a query is registered.
    /// For that, it will search all registered non-generic query handlers and if only one is found for the query, that one will be used for the pipeline.
    /// </summary>
    public bool IsPipelineAutoRegistrationEnabled { get; }

    /// <summary>
    /// Starts query handler registry creation with a builder.
    /// </summary>
    [DebuggerStepThrough]
    public static IQueryHandlerRegistryBuilder Builder() => new QueryRegistryBuilder();

    public IReadOnlyList<Pipeline> GetAllPipelines()
        => _pipelineByQueryTypeDictionary.Values.ToList();

    [DebuggerStepThrough]
    public Pipeline GetPipelineForQuery(Type requestType, IQueryHandlerFactory queryHandlerFactory)
    {
        return _pipelineByQueryTypeDictionary.TryGetValue(requestType, out var pipeline)
            ? pipeline
            : (IsPipelineAutoRegistrationEnabled
                ? TryRegisterAndReturnAutoPipeline(requestType, queryHandlerFactory)
                : throw new MissingPipelineException(requestType));
    }

    [DebuggerStepThrough]
    public void RegisterPipeline(Pipeline pipeline)
        => _pipelineByQueryTypeDictionary.AddOrUpdate(pipeline.QueryType, pipeline, (_, _) => pipeline);

    private Pipeline TryRegisterAndReturnAutoPipeline(Type requestType, IQueryHandlerFactory queryHandlerFactory)
    {
        var queryType = requestType;
        var returnType = queryType.GetInterface("IQuery`2")?.GenericTypeArguments[0];
        if (queryType is null || returnType is null)
        {
            throw new MissingPipelineException(requestType, new ArgumentException($"Could not determine query return type from query type {requestType.FullName} for auto registration"));
        }

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, returnType);
        var createQueryHandlerMethod = queryHandlerFactory.GetType().GetMethod("CreateQueryHandler");
        var genericCreateQueryHandlerMethod = createQueryHandlerMethod!.MakeGenericMethod(handlerType, queryType, returnType);

        var handler = genericCreateQueryHandlerMethod.Invoke(queryHandlerFactory, new object[0]);
        if (handler is null)
        {
            throw new MissingPipelineException(requestType, new ArgumentException($"Could not get implementation for handler type {handlerType.Name} for auto registration"));
        }

        var pipeline = new Pipeline(queryType, returnType, new List<PipelinePart>()
        {
            new()
            {
                HandlerConfiguration = null,
                HandlerType = handlerType,
            }
        });

        _pipelineByQueryTypeDictionary.AddOrUpdate(requestType, pipeline, (_, _) => pipeline);

        return pipeline;
    }

    private class QueryRegistryBuilder : IQueryHandlerRegistryBuilder
    {
        private List<Assembly> _assembliesToSearchProfilesIn = new();
        private bool _isPipelineAutoRegistrationEnabled;
        private List<Action<Action<Pipeline>>> _registerCallbacks = new();

        public IQueryHandlerRegistryBuilder AddProfiles(params Assembly[] assembliesToSearchProfilesIn)
        {
            _assembliesToSearchProfilesIn.AddRange(assembliesToSearchProfilesIn);
            return this;
        }

        public IQueryHandlerRegistryBuilder AddRegisterCallback(Action<Action<Pipeline>> registerCallback)
        {
            _registerCallbacks.Add(registerCallback);
            return this;
        }

        public IQueryHandlerRegistry Build()
        {
            var registry = new QueryHandlerRegistry(_isPipelineAutoRegistrationEnabled);
            foreach (var assembly in _assembliesToSearchProfilesIn)
            {
                foreach (var profileType in assembly.GetTypes().Where(t => typeof(IPipelineProfile).IsAssignableFrom(t)))
                {
                    var profile = Activator.CreateInstance(profileType) as IPipelineProfile;
                    profile!.RegisterPipelines(registry.RegisterPipeline);
                }
            }

            foreach (var registerCallback in _registerCallbacks)
            {
                registerCallback(registry.RegisterPipeline);
            }

            return registry;
        }

        [DebuggerStepThrough]
        public IQueryHandlerRegistryBuilder WithPipelineAutoRegistration()
        {
            _isPipelineAutoRegistrationEnabled = true;
            return this;
        }
    }
}
