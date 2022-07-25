namespace Prodot.Patterns.Cqrs;

public sealed class PipelineBuilder<TQuery, TResult> where TQuery : IQuery<TResult, TQuery>
{
    private readonly List<PipelinePart> _pipelineDescriptors = new();

    public Pipeline Build()
    {
        if (_pipelineDescriptors.Count == 0)
        {
            throw new InvalidOperationException("Can not create empty pipeline.");
        }

        return new Pipeline(typeof(TQuery), typeof(TResult), _pipelineDescriptors);
    }

    public PipelineBuilder<TQuery, TResult> With<T>() where T : IQueryHandler<TQuery, TResult>
    {
        _pipelineDescriptors.Add(new PipelinePart
        {
            HandlerType = typeof(T),
            HandlerConfiguration = null
        });
        return this;
    }

    public PipelineBuilder<TQuery, TResult> With<T, TConfiguration>(TConfiguration configuration) where T : IQueryHandler<TQuery, TResult>, IConfigurableQueryHandler<TConfiguration>
    {
        _pipelineDescriptors.Add(new PipelinePart
        {
            HandlerType = typeof(T),
            HandlerConfiguration = configuration
        });
        return this;
    }
}
