namespace Prodot.Patterns.Cqrs;

public interface IQueryHandlerRegistry
{
    IReadOnlyList<Pipeline> GetAllPipelines();

    Pipeline GetPipelineForQuery(Type queryType, IQueryHandlerFactory queryHandlerFactory);
}
