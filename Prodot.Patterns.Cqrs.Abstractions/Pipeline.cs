namespace Prodot.Patterns.Cqrs;

public sealed class Pipeline
{
    internal Pipeline(Type queryType, Type resultType, IReadOnlyList<PipelinePart> parts)
    {
        QueryType = queryType;
        ResultType = resultType;
        Parts = parts;
    }

    public IReadOnlyList<PipelinePart> Parts { get; }

    public Type QueryType { get; }

    public Type ResultType { get; }
}
