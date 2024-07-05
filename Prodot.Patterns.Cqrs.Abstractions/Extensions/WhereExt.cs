// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to filter instances of Option.
/// </summary>
public static class WhereExt
{
    /// <summary>
    ///     Returns the specified option if the specified predicate holds, otherwise None.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The predicate argument is null.
    /// </exception>
    public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate)
    {
        predicate.ThrowIfNull(nameof(predicate));

        return option.Match(none: () => option, some: x => predicate(x) ? x : Option<T>.None);
    }
}
