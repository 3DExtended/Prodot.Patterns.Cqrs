// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to convert a option with a nullable type into the value representation
///     of that type.
/// </summary>
public static class NormalizeExt
{
    /// <summary>
    ///     Normalizes the specified <paramref name="option" /> of a nullable type into the value representation of that type.
    /// </summary>
    public static Option<T> Normalize<T>(this Option<T?> option)
        where T : struct => option.Match(none: () => Option.None, some: Option.From);
}
