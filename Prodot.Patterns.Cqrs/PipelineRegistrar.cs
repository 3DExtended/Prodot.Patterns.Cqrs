namespace Prodot.Patterns.Cqrs;

public class PipelineRegistrar
{
    private readonly Action<Pipeline> _registerFunction;

    public PipelineRegistrar(Action<Pipeline> registerFunction)
    {
        _registerFunction = registerFunction;
    }

    public static implicit operator PipelineRegistrar(Action<Pipeline> registerFunction)
        => new PipelineRegistrar(registerFunction);

    public void Register<TQuery, TResult>(Action<PipelineBuilder<TQuery, TResult>> builderConfig)
        where TQuery : IQuery<TResult, TQuery>
    {
        var builder = new PipelineBuilder<TQuery, TResult>();
        builderConfig(builder);
        var pipeline = builder.Build();
        _registerFunction(pipeline);
    }
}
