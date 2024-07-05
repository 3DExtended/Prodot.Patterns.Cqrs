namespace Prodot.Patterns.Cqrs;

public sealed class PipelinePart
{
    public object? HandlerConfiguration { get; init; }

    public Type HandlerType { get; init; } = default!;
}
