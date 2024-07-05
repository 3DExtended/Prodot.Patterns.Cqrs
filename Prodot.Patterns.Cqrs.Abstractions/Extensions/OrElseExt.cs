// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to provide fallbacks when an option is None.
/// </summary>
public static class OrElseExt
{
    /// <summary>
    ///     If the specified option contains no value, the given fallback
    ///     value will be returned, wrapped in an option.
    /// </summary>
    public static Option<T> OrElse<T>(this Option<T> option, T fallback) =>
        OrElseWith(option, () => Option.From(fallback));

    /// <summary>
    ///     If the specified option contains no value, the given fallback func is
    ///     executed and the produced value will be returned, wrapped in an option.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The fallback argument is null.
    /// </exception>
    public static Option<T> OrElse<T>(this Option<T> option, Func<T> fallback)
    {
        fallback.ThrowIfNull(nameof(fallback));

        return OrElseWith(option, () => Option.From(fallback()));
    }

    /// <summary>
    ///     If the specified option contains no value, the given fallback option is returned.
    /// </summary>
    public static Option<T> OrElseWith<T>(this Option<T> option, Option<T> fallback) =>
        OrElseWith(option, () => fallback);

    /// <summary>
    ///     If the specified option contains no value, the given fallback func
    ///     is executed and the produced option is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The fallback argument is null.
    /// </exception>
    public static Option<T> OrElseWith<T>(this Option<T> option, Func<Option<T>> fallback)
    {
        fallback.ThrowIfNull(nameof(fallback));

        return option.IsSome ? option : fallback();
    }
}
