// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to check if a specified option contains a specific value.
/// </summary>
public static class ContainsExt
{
    /// <summary>
    ///     Returns a value that indicates if the specified option contains the desired value.
    /// </summary>
    public static bool Contains<T>(this Option<T> option, T desiredValue, IEqualityComparer<T>? comparer = null)
    {
        var c = comparer ?? EqualityComparer<T>.Default;

        return option.Contains(desiredValue, c.Equals);
    }

    /// <summary>
    ///    Returns a value that indicates if the specified option contains the desired value.
    ///    The specified compare function is used to check for equality.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The compare argument is null.
    /// </exception>
    public static bool Contains<T>(this Option<T> option, T desiredValue, Func<T, T, bool> compare)
    {
        compare.ThrowIfNull(nameof(compare));

        return option.Match(
            none: () => false,
            some: x => compare(x, desiredValue));
    }
}
