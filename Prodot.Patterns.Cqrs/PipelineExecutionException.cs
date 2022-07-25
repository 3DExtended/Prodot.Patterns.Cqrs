namespace Prodot.Patterns.Cqrs;

public class PipelineExecutionException : Exception
{
    public PipelineExecutionException(object query)
        : base($"Execution of query '{query.GetType().FullName}' returned None.")
        => ExecutedQuery = query;

    public PipelineExecutionException(object query, Exception innerException)
        : base($"Execution of query '{query.GetType().FullName}' returned None.", innerException)
        => ExecutedQuery = query;

    public object ExecutedQuery { get; }
}
