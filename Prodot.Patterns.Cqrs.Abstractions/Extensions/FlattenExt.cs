// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to extract instances of nested Option.
/// </summary>
public static class FlattenExt
{
    /// <summary>
    ///     Returns the nested option from the specified <paramref name="nestedOption" /> or None, if nothing
    ///     is contained.
    /// </summary>
    public static Option<T> Flatten<T>(this Option<Option<T>> nestedOption) =>
        nestedOption.Match(
            none: () => Option.None,
            some: x => x);
}
