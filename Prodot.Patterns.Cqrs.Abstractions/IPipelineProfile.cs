namespace Prodot.Patterns.Cqrs;

public interface IPipelineProfile
{
    void RegisterPipelines(Action<Pipeline> registerFunction);
}
