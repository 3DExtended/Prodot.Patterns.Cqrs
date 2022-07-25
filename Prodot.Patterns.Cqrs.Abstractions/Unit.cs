namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Unit represents an empty value.
/// </summary>
public readonly record struct Unit
{
    /// <summary>
    /// The globally unique empty Unit value.
    /// </summary>
    public static Unit Value { get; }
}
