namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Indicates that no pipeline registration was found for a given query type.
/// </summary>
public class MissingPipelineException : Exception
{
    public MissingPipelineException(Type queryType)
        : base($"Could not find pipeline for query type '{queryType.FullName}'.") =>
        QueryType = queryType;

    public MissingPipelineException(Type queryType, Exception innerException)
        : base($"Could not find pipeline for query type '{queryType.FullName}'.", innerException) =>
        QueryType = queryType;

    public Type QueryType { get; }
}
